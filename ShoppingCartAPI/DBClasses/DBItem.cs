using ShoppingCartAPI.DB;
using ShoppingCartAPI.Models;
using ShoppingCartAPI.Utils;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;

namespace ShoppingCartAPI.DBClasses
{
    public class DBItem
    {
        private string token = "";

        public DBItem(string received_token)
        {
            token = received_token;
        }

        /// <summary>
        /// Get all items available in stock
        /// </summary>
        /// <param name="items">list of items to be returned</param>
        /// <returns>whether call to DB was successful or not</returns>
        public bool GetItems(out List<ItemModel> items)
        {
            bool success = false;
            items = new List<ItemModel>();

            try { 
                using (ShoppingCartEntities context = new ShoppingCartEntities())
                {
                    items = context.Items
                        .Where(i => i.Quantity != 0) //Get available items only
                        .Select(i => new ItemModel()
                        {
                            ID = i.ID,
                            Name = i.Name,
                            Description = i.Description,
                            Quantity = i.Quantity,
                            Price = i.Price
                        })
                        .ToList();
                }
                
                success = true;            
            }
            catch(Exception ex)
            {
                new Logging().LogProgress(token, Common.CallerIP, ex);
            }

            return success;
        }

        /// <summary>
        /// Get all items in cart
        /// </summary>
        /// <param name="cartId">Id of cart</param>
        /// <param name="items">outputs a list of items found in cart</param>
        /// <returns>whether the task was successful or not</returns>
        public bool GetCartItems(int cartId, out List<CartItemModel> items)
        {
            bool success = false;
            items = new List<CartItemModel>();

            try
            {
                using (ShoppingCartEntities context = new ShoppingCartEntities())
                { 
                    //Get items for cart
                    items = context.CartItems
                        .Include("Item")
                        .Where(i => i.CartId == cartId)
                        .Select(i => new CartItemModel()
                        {
                            ID = i.ID,
                            CartId = i.CartId,
                            ItemId = i.ItemId,
                            Name = i.Item.Name,
                            Description = i.Item.Description,
                            Quantity = i.Quantity,
                            Price = i.Item.Price
                        })
                        .ToList();
                }

                success = true;
            }
            catch (Exception ex)
            {
                new Logging().LogProgress(token, Common.CallerIP, ex);
            }

            return success;
        }

        /// <summary>
        /// Add item to cart
        /// </summary>
        /// <param name="cartItem">item to be added in cart</param>
        /// <returns>whether the task was succesful or not</returns>
        public bool AddCartItem(CartItemModel cartItem, out HttpStatusCode status)
        {
            bool success = false;
            status = HttpStatusCode.OK;

            try
            {
                CartItem newCartItem = new CartItem();
                newCartItem.CartId = cartItem.CartId;
                newCartItem.ItemId = cartItem.ItemId;
                newCartItem.Quantity = cartItem.Quantity;

                using (ShoppingCartEntities context = new ShoppingCartEntities())
                {
                    //check if item already exists in cart
                    CartItem cartExists = context.CartItems
                        .Include("Item")
                        .Where(c => c.ItemId == newCartItem.ItemId && c.CartId == newCartItem.CartId)
                        .FirstOrDefault();

                    if (cartExists == null)
                    {
                        //Add new item to car
                        context.CartItems.Add(newCartItem);
                        context.SaveChanges();
                        success = true;
                    }
                    else
                    {
                        if(cartExists.Quantity + newCartItem.Quantity > cartExists.Item.Quantity)
                        {
                            //HTTP 410 - Not enough quantity in stock
                            status = HttpStatusCode.Gone;
                        }
                        else
                        {
                            //Update quantity of item in cart
                            cartExists.Quantity += newCartItem.Quantity;
                            context.CartItems.Attach(cartExists);
                            context.Entry(cartExists).State = EntityState.Modified;
                            context.SaveChanges();
                            success = true;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                new Logging().LogProgress(token, Common.CallerIP, ex);
            }

            return success;
        }

        /// <summary>
        /// Delete item from cart
        /// </summary>
        /// <param name="id">Id of item to delete</param>
        /// <param name="status">outputs status</param>
        /// <returns>whether deletion was successful or not</returns>
        public bool DeleteCartItem(int id, out HttpStatusCode status)
        {
            bool success = false;
            status = HttpStatusCode.OK;

            try
            {
                using (ShoppingCartEntities context = new ShoppingCartEntities())
                {
                    //Get cart item for deletion
                    CartItem deleteItem = context.CartItems
                               .Where(i => i.ID == id).FirstOrDefault();

                    if(deleteItem != null)
                    {
                        //Delete item from cart
                        context.CartItems.Remove(deleteItem);
                        context.SaveChanges();

                        success = true;
                    }
                    else
                    {
                        status = HttpStatusCode.NotFound;
                    }
                }
            }
            catch (Exception ex)
            {
                status = HttpStatusCode.InternalServerError;
                new Logging().LogProgress(token, Common.CallerIP, ex);
            }

            return success;
        }

        /// <summary>
        /// Delete all items from cart
        /// </summary>
        /// <param name="id">Id of cart to delete items from</param>
        /// <param name="status">outputs status</param>
        /// <returns>whether deletion was successful or not</returns>
        public bool DeleteAllCartItems(int id, out HttpStatusCode status)
        {
            bool success = false;
            status = HttpStatusCode.OK;

            try
            {
                using (ShoppingCartEntities context = new ShoppingCartEntities())
                {
                    //Get cart
                    Cart cart = context.Carts
                               .Where(i => i.ID == id).FirstOrDefault();

                    if (cart != null)
                    {
                        //Remove all items in cart
                        context.CartItems.RemoveRange(context.CartItems.Where(c => c.CartId == id));
                        context.SaveChanges();

                        success = true;
                    }
                    else
                    {
                        status = HttpStatusCode.NotFound;
                    }
                }
            }
            catch (Exception ex)
            {
                status = HttpStatusCode.InternalServerError;
                new Logging().LogProgress(token, Common.CallerIP, ex);
            }

            return success;
        }
    }
}