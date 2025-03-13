using System.Web.Http;
using Owin;
using Microsoft.Owin;
using System.Web.Http.Cors;

namespace WinFormEImza
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            // Configure Web API
            var config = new HttpConfiguration();

            // Configure routes
            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{action}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );

            // Enable CORS
            var cors = new EnableCorsAttribute("*", "*", "*");
            config.EnableCors(cors);

            // Configure JSON serializer
            config.Formatters.JsonFormatter.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;

            // Use Web API middleware
            app.UseWebApi(config);
        }
    }
}