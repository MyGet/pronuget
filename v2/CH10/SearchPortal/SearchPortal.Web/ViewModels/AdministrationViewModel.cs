using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SearchPortal.Web.ViewModels
{
    public class AdministrationViewModel
    {
        public AdministrationViewModel()
        {
            Plugins = new List<PluginViewModel>();
        }

        public List<PluginViewModel> Plugins { get; set; }
    }
}