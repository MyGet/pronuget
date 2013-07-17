using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SearchPortal.Web.Models
{
    public class PluginModel
    {
        public string PackageId { get; set; }
        public string PackageVersion { get; set; }
        public string PackageDescription { get; set; }
        public bool IsInstalled { get; set; }
    }
}