using System.Configuration;
using System.Web;

namespace ShoppingCartWeb.Utils
{
    public static class Common
    {
        public static string API_URL
        {
            get
            { 
                return Properties.Settings.Default.API_URL;
            }
        }
        public static string Token
        {
            get
            {
                return Properties.Settings.Default.Token;
            }
        }
    }
}