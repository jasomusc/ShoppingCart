using System.Configuration;
using System.Web;

namespace ShoppingCartAPI.Utils
{
    public static class Common
    {
        public static string Token
        {
            get
            { 
                return Properties.Settings.Default.Token;
            }
        }

        public static string CallerIP
        {
            get
            {
                try
                {
                    string ip = HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];

                    if (string.IsNullOrEmpty(ip))
                    {
                        ip = HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];
                    }

                    return ip;
                }
                catch
                {
                    return "Unit Test";
                }
            }
        }
    }
}