using System.ComponentModel.Composition;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace NuGet.PackageAnalysis.Certificates
{
    [Export(typeof(IPackageRule))]
    public sealed class CertificateRule : IPackageRule
    {
        private readonly string[] _forbiddenExtensions = { ".cer", ".pfx" };

        public IEnumerable<PackageIssue> Validate(IPackage package)
        {
            // Loop through all files and match them with _forbiddenExtensions
            foreach (var packageFile in package.GetFiles())
            {
                if (_forbiddenExtensions.Any(
                    extension => extension == Path.GetExtension(packageFile.Path).ToLowerInvariant()))
                {
                    yield return new PackageIssue(
                        "Forbidden file extension found", 
                        string.Format("The file {0} is of a forbidden file extension and may not be included in a NuGet package.", packageFile.Path), 
                        "Remove it from the package folder.", 
                        PackageIssueLevel.Error);
                }
            }
        }
    }
}
