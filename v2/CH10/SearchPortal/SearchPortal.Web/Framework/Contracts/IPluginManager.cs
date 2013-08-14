using System.Collections.Generic;
using System.ComponentModel.Composition;
using SearchPortal.Web.Framework.Models;

namespace SearchPortal.Web.Framework.Contracts
{
    public interface IPluginManager
    {
        IList<PluginModel> ListPlugins();
        void Install(string packageId, string packageVersion);
        void Uninstall(string packageId, string packageVersion);
    }
}