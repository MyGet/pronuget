using System;
using System.ComponentModel.Composition;

namespace SearchPortal.Web.Code.Contracts
{
    [InheritedExport]
    public interface ISearchEngine
    {
        string Name { get; }

        Uri GenerateSearchUri(string searchTerm);
    }
}