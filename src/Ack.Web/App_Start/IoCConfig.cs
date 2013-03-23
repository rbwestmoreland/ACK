using Ack.Web.Controllers.Mvc;
using Ack.Web.Controllers.Mvc.ControllerFactories;
using Ack.Web.Controllers.WebApi;
using Ack.Web.Controllers.WebApi.DependencyResolvers;
using System.Web.Http;
using System.Web.Http.Filters;
using System.Web.Mvc;
using TinyIoC;

namespace Ack.Web
{
    public static class IoCConfig
    {
        private static TinyIoCContainer Container { get; set; }

        static IoCConfig()
        {
            Container = new TinyIoCContainer();
            RegisterCommon();
        }

        private static void RegisterCommon()
        {
        }

        public static void RegisterMvc(ControllerBuilder controllerBuilder, GlobalFilterCollection globalFilters)
        {
            //controllers
            Container.Register<IController, HomeController>("Home").AsMultiInstance();

            //filters
            var filters = Container.ResolveAll<IMvcFilter>();
            foreach (var filter in filters)
            {
                globalFilters.Add(filter);
            }

            //controller factory
            controllerBuilder.SetControllerFactory(new TinyIocControllerFactory(Container));
        }

        public static void RegisterWebApi(HttpConfiguration config)
        {
            //controllers
            Container.Register<ValuesController>().AsMultiInstance();

            //filters
            var filters = Container.ResolveAll<IFilter>();
            foreach (var filter in filters)
            {
                config.Filters.Add(filter);
            }

            //dependency resolver
            config.DependencyResolver = new TinyIoCDependencyResolver(Container);
        }
    }
}