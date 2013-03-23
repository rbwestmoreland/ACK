using System.Web;

namespace Ack.Web
{
    public class WebApiApplication : HttpApplication
    {
        protected void Application_Start()
        {
            ApplicationConfig.Initialize();
        }
    }
}