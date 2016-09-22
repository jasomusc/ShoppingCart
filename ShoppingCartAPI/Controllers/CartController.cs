using ShoppingCartAPI.DBClasses;
using ShoppingCartAPI.Utils;
using System;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace ShoppingCartAPI.Controllers
{
    public class CartController : ApiController
    {
        //GET api/cart/2
        public HttpResponseMessage Get(int id)
        {
            // HTTP 400 - Bad Request
            return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Bad Request");
        }

        // POST api/cart
        public HttpResponseMessage Post(string value)
        {
            // HTTP 400 - Bad Request
            return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Bad Request");
        }

        // PUT api/cart/5
        public HttpResponseMessage Put(int id, [FromBody]string value)
        {
            // HTTP 400 - Bad Request
            return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Bad Request");
        }

        // DELETE api/cart
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
                    bool success = db.DeleteAllCartItems(id, out result);

                    if (success)
                    {
                        // HTTP 200 - Saved Successfully
                        return Request.CreateErrorResponse(HttpStatusCode.OK, "All items deleted successfully from your cart");
                    }
                    else
                    {
                        if (result == HttpStatusCode.NotFound)
                        {
                            // HTTP 500 - Internal Error
                            return Request.CreateErrorResponse(result, "Cart not found");
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
