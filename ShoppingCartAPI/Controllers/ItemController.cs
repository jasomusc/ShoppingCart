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
    public class ItemController : ApiController
    {
        //GET api/item/5
        public HttpResponseMessage Get(string token)
        {
            try
            {
                // token correct
                if (token != null && Common.Token.Equals(token))
                {                    
                    List<ItemModel> items = new List<ItemModel>();
                    DBItem db = new DBItem(token);

                    // Accessing the DB to get the Items
                    bool success = db.GetItems(out items);

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
                            return Request.CreateErrorResponse(HttpStatusCode.NotFound, "No Items Found");
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

        // POST api/item
        public HttpResponseMessage Post(string value)
        {
            // HTTP 400 - Bad Request
            return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Bad Request");
        }

        // PUT api/item/5
        public HttpResponseMessage Put(int id, [FromBody]string value)
        {
            // HTTP 400 - Bad Request
            return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Bad Request");
        }

        // DELETE api/item/5
        public HttpResponseMessage Delete(int id)
        {
            // HTTP 400 - Bad Request
            return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Bad Request");
        }
    }
}
