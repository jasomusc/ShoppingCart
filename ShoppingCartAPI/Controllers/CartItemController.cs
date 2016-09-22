using ShoppingCartAPI.DBClasses;
using ShoppingCartAPI.Models;
using ShoppingCartAPI.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;

namespace ShoppingCartAPI.Controllers
{
    public class CartItemController : ApiController
    {
        // GET api/cartitem/5
        public HttpResponseMessage Get(string token, int id)
        {
            try
            {
                // token correct
                if (token != null && Common.Token.Equals(token))
                {                    
                    List<CartItemModel> items = new List<CartItemModel>();
                    DBItem db = new DBItem(token);

                    // Accessing the DB to get the Items
                    bool success = db.GetCartItems(id, out items);

                    if (success)
                    {
                        // HTTP 200 - Cart found
                        if (items.Count() != 0)
                        {
                            var response = Request.CreateResponse(HttpStatusCode.OK);
                            response.Content = new StringContent(Newtonsoft.Json.JsonConvert.SerializeObject(items), System.Text.Encoding.UTF8, "application/json");
                            return response;
                        }
                        // HTTP 404 - Cart not found
                        else
                        {
                            return Request.CreateErrorResponse(HttpStatusCode.NotFound, "No Cart Items Found");
                        }
                    }
                    else
                    {
                        // HTTP 500 - Internal Error
                        return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Internal Error");
                    }
                }
                else
                {
                    new Logging().LogProgress(token, Common.CallerIP, new Exception("Invalid token passed"));

                    // HTTP 400 - Bad Request (token incorrect)
                    return Request.CreateErrorResponse(HttpStatusCode.Forbidden, "Invalid token passed");
                }
            }
            catch (Exception ex)
            {
                new Logging().LogProgress(token, Common.CallerIP, ex);

                // HTTP 500 - Internal Error
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Internal Error");
            }
        }

        // POST api/cartitem
        public HttpResponseMessage Post([FromBody] CartItemModel cartItem)
        {
            try
            {
                // token correct
                if (cartItem != null && cartItem.Token != null && Common.Token.Equals(cartItem.Token))
                {
                    if (ModelState.IsValid)
                    {
                        DBItem db = new DBItem(cartItem.Token);
                        HttpStatusCode status;

                        // Add cart item in DB
                        bool success = db.AddCartItem(cartItem, out status);

                        if (success)
                        {
                            // HTTP 200 - Saved Successfully
                            return Request.CreateErrorResponse(HttpStatusCode.OK, "Item added successfully to your cart");
                        }
                        else
                        {
                            if (status == HttpStatusCode.Gone)
                            {
                                // HTTP 410 - Not enough quantity in stock
                                return Request.CreateErrorResponse(HttpStatusCode.ExpectationFailed, "Not enough quantity in stock. Please check if you already have this item in the cart");
                            }
                            else
                            {
                                // HTTP 500 - Internal Error
                                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Internal Error");
                            }
                        }
                    }
                    else
                    {
                        //validation errors
                        string message = string.Join(", ", ModelState.Values
                            .SelectMany(x => x.Errors)
                            .Select(x => x.ErrorMessage));

                        return Request.CreateErrorResponse(HttpStatusCode.ExpectationFailed, message);
                    }
                }
                else
                {
                    new Logging().LogProgress(cartItem.Token, Common.CallerIP, new Exception("Invalid token passed"));

                    // HTTP 400 - Bad Request (token incorrect)
                    return Request.CreateErrorResponse(HttpStatusCode.Forbidden, "Invalid token passed");
                }
            }
            catch (Exception ex)
            {
                new Logging().LogProgress(cartItem.Token, Common.CallerIP, ex);

                // HTTP 500 - Internal Error
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Internal Error");
            }
        }
        
        // PUT api/cartitem/5
        public HttpResponseMessage Put(int id, [FromBody]string value)
        {
            // HTTP 400 - Bad Request
            return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Bad Request");
        }

        // DELETE api/cartitem
        public HttpResponseMessage Delete(string token, int id)
        {
            try
            {
                // token correct
                if (token != null && Common.Token.Equals(token))
                {
                    DBItem db = new DBItem(token);

                    // Add cart item in DB
                    HttpStatusCode result;
                    bool success = db.DeleteCartItem(id, out result);

                    if (success)
                    {
                        // HTTP 200 - Saved Successfully
                        return Request.CreateErrorResponse(HttpStatusCode.OK, "Item deleted successfully from your cart");
                    }
                    else
                    {
                        if (result == HttpStatusCode.NotFound)
                        {
                            // HTTP 500 - Internal Error
                            return Request.CreateErrorResponse(result, "Cart item not found");
                        }
                        else
                        { 
                            // HTTP 500 - Internal Error
                            return Request.CreateErrorResponse(result, "Internal Error");
                        }
                    }
                }
                else
                {
                    new Logging().LogProgress(token, Common.CallerIP, new Exception("Invalid token passed"));

                    // HTTP 400 - Bad Request (token incorrect)
                    return Request.CreateErrorResponse(HttpStatusCode.Forbidden, "Invalid token passed");
                }

            }
            catch (Exception ex)
            {
                new Logging().LogProgress(token, Common.CallerIP, ex);

                // HTTP 500 - Internal Error
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Internal Error");
            }
        }
    }
}
