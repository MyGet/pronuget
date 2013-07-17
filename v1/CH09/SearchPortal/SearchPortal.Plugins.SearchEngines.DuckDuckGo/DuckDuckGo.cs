using System;
using SearchPortal.Web.Code.Contracts;

namespace SearchPortal.Plugins.SearchEngines.DuckDuckGo
{
    public class DuckDuckGo
        : ISearchEngine
    {
        public string Name
        {
            get { return "DuckDuckGo"; }
        }

        public Uri GenerateSearchUri(string searchTerm)
        {
            return new Uri(string.Format("http://duckduckgo.com/?q={0}", searchTerm));
        }
    }
}
