using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Hosting;
using NuGet;
using SearchPortal.Web.Code.Contracts;
using SearchPortal.Web.Models;

namespace SearchPortal.Web.Code
{
    public class PluginManager
        : IPluginManager
    {
        private readonly string _pluginFolder;
        private readonly IPackageRepository _packageRepository;
        private readonly PackageManager _packageManager;

        public PluginManager()
        {
            _pluginFolder = HostingEnvironment.MapPath("~/App_Data/Plugins");
            _packageRepository = PackageRepositoryFactory.Default.CreateRepository("http://www.myget.org/F/pronuget/");
            _packageManager = new PackageManager(_packageRepository, _pluginFolder);
        }

        public IEnumerable<PluginModel> ListPlugins()
        {
            IPackage dummy = null;

            return _packageManager.SourceRepository.GetPackages()
                .Where(p => p.Tags.Contains("chapter9plugin"))
                .OrderBy(p => p.Id)
                .ToList()
                .Select(p => new PluginModel()
                                 {
                                     PackageId = p.Id,
                                     PackageVersion = p.Version.ToString(),
                                     PackageDescription = p.Description,
                                     IsInstalled = _packageManager.LocalRepository.TryFindPackage(p.Id, p.Version, out dummy)
                                 })
                .ToList();
        }

        public void Install(string packageId, string packageVersion)
        {
            _packageManager.InstallPackage(packageId, new Version(packageVersion));

            HostingEnvironment.InitiateShutdown();
        }

        public void Uninstall(string packageId, string packageVersion)
        {
            _packageManager.UninstallPackage(packageId, new Version(packageVersion));

            HostingEnvironment.InitiateShutdown();
        }
    }
}