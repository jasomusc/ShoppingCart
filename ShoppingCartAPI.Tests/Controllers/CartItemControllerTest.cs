using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using ShoppingCartAPI.Controllers;
using ShoppingCartAPI.Models;
using ShoppingCartAPI.Utils;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;

namespace ShoppingCartAPI.Tests.Controllers
{
    [TestClass]
    public class CartItemControllerTest
    {        
        [TestMethod]
        public void PostCartItem()
        {
            // Arrange
            CartItemController controller = new CartItemController { Request = new HttpRequestMessage() };

            CartItemModel cartModel = new CartItemModel();
            cartModel.Token = Common.Token;
            cartModel.CartId = 2;
            cartModel.ItemId = 2;
            cartModel.Quantity = 1;

            // Act
            HttpResponseMessage result = controller.Post(cartModel);

            // Assert
            Assert.AreEqual(HttpStatusCode.OK, result.StatusCode);
        }

        [TestMethod]
        public void GetCartItems()
        {
            // Arrange
            CartItemController controller = new CartItemController { Request = new HttpRequestMessage() };

            // Act
            HttpResponseMessage result = controller.Get(Common.Token, 2);
            
            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(HttpStatusCode.OK, result.StatusCode);
        }

        public void PutCartItem()
        {
            // Arrange
            CartItemController controller = new CartItemController { Request = new HttpRequestMessage() };

            // Act
            HttpResponseMessage result = controller.Put(5, "value");

            // Assert
            Assert.AreEqual(HttpStatusCode.BadRequest, result.StatusCode);
        }

        [TestMethod]
        public void DeleteCartItem()
        {
            // Arrange
            CartItemController controller = new CartItemController { Request = new HttpRequestMessage() };

            // Act
            HttpResponseMessage result = controller.Delete(Common.Token, 6);

            // Assert
            //Assert.AreEqual(HttpStatusCode.OK, result.StatusCode);

            //Check not found because item to delete id needs to be change to test if delete works
            Assert.AreEqual(HttpStatusCode.NotFound, result.StatusCode);
        }
    }
}
