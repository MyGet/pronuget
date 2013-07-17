using System;
using SearchPortal.Web.Code.Contracts;

namespace SearchPortal.Plugins.SearchEngines.Google
{
    public class Google
        : ISearchEngine
    {
        public string Name
        {
            get { return "Google"; }
        }

        public Uri GenerateSearchUri(string searchTerm)
        {
            return new Uri(string.Format("http://www.google.com/search?q={0}", searchTerm));
        }
    }
}
