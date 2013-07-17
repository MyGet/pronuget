﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SearchPortal.Web.Framework;
using SearchPortal.Web.ViewModels;

namespace SearchPortal.Web.Controllers
{
    [Authorize]
    public class AdministrationController : Controller
    {
        private readonly PluginManager _pluginManager;

        public AdministrationController()
        {
            _pluginManager = new PluginManager();
        }

        public ActionResult Index()
        {
            var plugins = _pluginManager.ListPlugins();

            var viewModel = new AdministrationViewModel();
            viewModel.Plugins = plugins
                .Select(p => new PluginViewModel()
                {
                    PackageId = p.PackageId,
                    PackageVersion = p.PackageVersion,
                    PackageDescription = p.PackageDescription,
                    IsInstalled = p.IsInstalled
                })
                .ToList();

            return View(viewModel);
        }

        public ActionResult Install(string packageId, string packageVersion)
        {
            _pluginManager.Install(packageId, packageVersion);

            return RedirectToAction("Index");
        }

        public ActionResult Uninstall(string packageId, string packageVersion)
        {
            _pluginManager.Uninstall(packageId, packageVersion);

            return RedirectToAction("Index");
        }
    }
}
