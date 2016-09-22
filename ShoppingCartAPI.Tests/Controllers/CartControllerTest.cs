using Microsoft.VisualStudio.TestTools.UnitTesting;
using ShoppingCartAPI.Controllers;
using ShoppingCartAPI.Utils;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Hosting;

namespace ShoppingCartAPI.Tests.Controllers
{
    [TestClass]
    public class CartControllerTest
    {
        [TestMethod]
        public void GetCart()
        {
            // Arrange
            CartController controller = new CartController { Request = new HttpRequestMessage() };

            // Act
            HttpResponseMessage result = controller.Get(5);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(HttpStatusCode.BadRequest, result.StatusCode);
        }

        [TestMethod]
        public void PostCart()
        {
            // Arrange
            CartController controller = new CartController { Request = new HttpRequestMessage() };

            // Act
            HttpResponseMessage result = controller.Post("");

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(HttpStatusCode.BadRequest, result.StatusCode);
        }

        [TestMethod]
        public void PutCart()
        {
            // Arrange
            CartController controller = new CartController { Request = new HttpRequestMessage() };

            // Act
            HttpResponseMessage result = controller.Put(5, "value");

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(HttpStatusCode.BadRequest, result.StatusCode);
        }

        [TestMethod]
        public void DeleteAllCartItems()
        {
            // Arrange
            CartController controller = new CartController { Request = new HttpRequestMessage() };

            // Act
            HttpResponseMessage result = controller.Delete(Common.Token, 2);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(HttpStatusCode.OK, result.StatusCode);
        }
    }
}
