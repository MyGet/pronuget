using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.IO;
using System.Linq;
using System.Reflection.Emit;
using System.Threading;
using System.Web;
using System.Web.Hosting;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using MefContrib.Hosting;
using SearchPortal.Web.Framework.Extensions;

namespace SearchPortal.Web
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : System.Web.HttpApplication
    {
        public static CompositionContainer Container { get; private set; }

        protected void Application_Start()
        {
            // Load plugins
            ShadowCopyPlugins();

            var catalog = new AggregateCatalog(
                new AssemblyCatalog(typeof(MvcApplication).Assembly),
                new RecursiveDirectoryCatalog(HostingEnvironment.MapPath("~/App_Data/ShadowedPlugins")));
            Container = new CompositionContainer(catalog);
            Container.ComposeParts(this);

            AreaRegistration.RegisterAllAreas();

            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }

        private static void ShadowCopyPlugins()
        {
            // Shadow copy plugins so that we can safely remove them without being affected by the CLR locking DLLs.
            var shadowedPlugins = new DirectoryInfo(HostingEnvironment.MapPath("~/App_Data/ShadowedPlugins"));
            if (shadowedPlugins.Exists)
            {
                var retries = 0;
                while (retries < 5)
                {
                    try
                    {
                        shadowedPlugins.Delete(true);
                    }
                    catch
                    {
                        retries++;
                        Thread.Sleep(TimeSpan.FromMilliseconds(250));
                    }
                }
            }
            shadowedPlugins.Create();
            var plugins = new DirectoryInfo(HostingEnvironment.MapPath("~/App_Data/Plugins"));
            if (plugins.Exists)
            {
                plugins.Copy(shadowedPlugins);
            }
        }
    }
}