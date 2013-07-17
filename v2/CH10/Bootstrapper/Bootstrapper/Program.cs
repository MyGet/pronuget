using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NuGet;

namespace Bootstrapper
{
    class Program
    {
        private const string ApplicationFeedUrl = "https://www.myget.org/F/pronuget/";
        private const string ApplicationPackageName = "Bootstrapper.SampleApp";
        private const string ApplicationFolder = "application";
        private const string ApplicationExecutableName = "Bootstrapper.SampleApp.exe";

        static void Main(string[] args)
        {
            var packagesFolder = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, ApplicationFolder);
            var packageRepository = PackageRepositoryFactory.Default.CreateRepository(ApplicationFeedUrl);
            var packageManager = new PackageManager(packageRepository, packagesFolder);
            var versionFile = Path.Combine(ApplicationFolder, "version.txt");

            if (!Directory.Exists(packagesFolder) || !Directory.EnumerateDirectories(packagesFolder, ApplicationPackageName + "*", SearchOption.AllDirectories).Any())
            {
                Console.WriteLine("Installing {0}...", ApplicationExecutableName);
                IPackage package = packageRepository.FindPackage(ApplicationPackageName);
                File.WriteAllText(versionFile, package.Version.ToString());
                packageManager.InstallPackage(ApplicationPackageName);
            }
            else
            {
                Console.WriteLine("Checking for {0} updates...", ApplicationExecutableName);

                var currentVersion = new SemanticVersion(File.ReadAllText(versionFile));
                IPackage package = packageRepository.FindPackage(ApplicationPackageName);
                if (package.Version > currentVersion)
                {
                    Console.WriteLine("Updating to version {0}...", package.Version);
                    packageManager.UpdatePackage(ApplicationPackageName, 
                        package.Version, false, false);
                    File.WriteAllText(versionFile, package.Version.ToString());
                }
                else
                {
                    Console.WriteLine("No updates were found.");
                }
            }

            // Find executable
            var executablePath = Directory.EnumerateFiles(packagesFolder, 
                ApplicationExecutableName, SearchOption.AllDirectories).FirstOrDefault();
            if (!string.IsNullOrEmpty(executablePath))
            {
                Console.WriteLine("Starting {0}...", ApplicationExecutableName);
                Process.Start(executablePath);
            }
            else
            {
                Console.WriteLine("Could not find {0} executable.", ApplicationExecutableName);
            } 
        }
    }
}
