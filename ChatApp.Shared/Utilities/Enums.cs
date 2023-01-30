using static System.Net.WebRequestMethods;

namespace ChatApp.Shared.Utilities
{
    public class Enums
    {
        public enum MyHttpMethods
        {
            GET,
            PUT,
            POST,
            DELETE,
            HEAD,
            OPTIONS,
            TRACE,
            PATCH,
            CONNECT
        }
    }
}
