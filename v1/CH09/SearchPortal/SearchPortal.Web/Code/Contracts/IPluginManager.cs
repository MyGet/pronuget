using System.Collections.Generic;
using System.ComponentModel.Composition;
using SearchPortal.Web.Models;

namespace SearchPortal.Web.Code.Contracts
{
    [InheritedExport]
    public interface IPluginManager
    {
        IEnumerable<PluginModel> ListPlugins();
        void Install(string packageId, string packageVersion);
        void Uninstall(string packageId, string packageVersion);
    }
}