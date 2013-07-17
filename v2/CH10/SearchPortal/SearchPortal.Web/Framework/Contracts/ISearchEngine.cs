using System;
using System.ComponentModel.Composition;

namespace SearchPortal.Web.Framework.Contracts
{
    [InheritedExport]
    public interface ISearchEngine
    {
        string Name { get; }

        Uri GenerateSearchUri(string searchTerm);
    }
}