using System;
using SearchPortal.Web.Framework.Contracts;

namespace SearchPortal.Plugins.SearchEngines.Bing
{
    public class Bing
        : ISearchEngine
    {
        public string Name
        {
            get { return "Bing"; }
        }

        public Uri GenerateSearchUri(string searchTerm)
        {
            return new Uri(string.Format("http://www.bing.com/search?q={0}", searchTerm));
        }
    }
}
