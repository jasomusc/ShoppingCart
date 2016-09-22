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
    public class ItemControllerTest
    {
        [TestMethod]
        public void GetItem()
        {
            // Arrange
            ItemController controller = new ItemController { Request = new HttpRequestMessage() };

            // Act
            HttpResponseMessage result = controller.Get(Common.Token);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(HttpStatusCode.OK, result.StatusCode);
        }
        
        public void PostItem()
        {
            // Arrange
            ItemController controller = new ItemController { Request = new HttpRequestMessage() };

            // Act
            HttpResponseMessage result = controller.Post("");

            // Assert
            Assert.AreEqual(HttpStatusCode.BadRequest, result.StatusCode);
        }
        
        public void PutItem()
        {
            // Arrange
            ItemController controller = new ItemController { Request = new HttpRequestMessage() };

            // Act
            HttpResponseMessage result = controller.Put(5, "value");

            // Assert
            Assert.AreEqual(HttpStatusCode.BadRequest, result.StatusCode);
        }
        
        public void DeleteItem()
        {
            // Arrange
            ItemController controller = new ItemController { Request = new HttpRequestMessage() };

            // Act
            HttpResponseMessage result = controller.Delete(6);

            // Assert
            Assert.AreEqual(HttpStatusCode.BadRequest, result.StatusCode);
        }
    }
}
