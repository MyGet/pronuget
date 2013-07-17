using System;
using System.Threading;
using System.Web.Compilation;
using System.Web.Hosting;
using MefContrib.Hosting;
using SearchPortal.Web.Code.Extensions;

[assembly: WebActivator.PreApplicationStartMethod(typeof(SearchPortal.Web.App_Start.AppStart_MefContribMVC3), "Start")]

namespace SearchPortal.Web.App_Start
{
    using System.ComponentModel.Composition.Hosting;
    using System.Linq;
    using System.Web.Mvc;
    using MefContrib.Hosting.Conventions;
    using MefContrib.Web.Mvc;
    using System.IO;

    public static class AppStart_MefContribMVC3
    {
        public static void Start()
        {
            // Shadow copy plugins so that we can safely remove them without being affected by the CLR locking DLLs.
            ShadowCopyPlugins();

            // Register the CompositionContainerLifetimeHttpModule HttpModule.
            // This makes sure everything is cleaned up correctly after each request.
            CompositionContainerLifetimeHttpModule.Register();

            // Create MEF catalog based on the contents of ~/bin.
            //
            // Note that any class in the referenced assemblies implementing in "IController"
            // is automatically exported to MEF. There is no need for explicit [Export] attributes
            // on ASP.NET MVC controllers. When implementing multiple constructors ensure that
            // there is one constructor marked with the [ImportingConstructor] attribute.
            var catalog = new AggregateCatalog(
                new DirectoryCatalog("bin"),
                new RecursiveDirectoryCatalog(HostingEnvironment.MapPath("~/App_Data/ShadowedPlugins")),
                new ConventionCatalog(new MvcApplicationRegistry())); // Note: add your own (convention)catalogs here if needed.

            // Tell MVC3 to use MEF as its dependency resolver.
            var dependencyResolver = new CompositionDependencyResolver(catalog);
            DependencyResolver.SetResolver(dependencyResolver);

            // Tell MVC3 to resolve dependencies in controllers
            ControllerBuilder.Current.SetControllerFactory(
                new CompositionControllerFactory(
                    new CompositionControllerActivator(dependencyResolver)));

            // Tell MVC3 to resolve dependencies in filters
            FilterProviders.Providers.Remove(FilterProviders.Providers.Single(f => f is FilterAttributeFilterProvider));
            FilterProviders.Providers.Add(new CompositionFilterAttributeFilterProvider(dependencyResolver));

            // Tell MVC3 to resolve dependencies in model validators
            ModelValidatorProviders.Providers.Remove(ModelValidatorProviders.Providers.OfType<DataAnnotationsModelValidatorProvider>().Single());
            ModelValidatorProviders.Providers.Add(
                new CompositionDataAnnotationsModelValidatorProvider(dependencyResolver));

            // Tell MVC3 to resolve model binders through MEF. Note that a model binder should be decorated
            // with [ModelBinderExport].
            ModelBinderProviders.BinderProviders.Add(
                new CompositionModelBinderProvider(dependencyResolver));
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