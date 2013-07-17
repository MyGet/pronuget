using System.IO;

namespace SearchPortal.Web.Framework.Extensions
{
    public static class DirectoryInfoExtensions
    {
        public static void Copy(this DirectoryInfo source, string destination)
        {
            Copy(source, new DirectoryInfo(destination));
        }

        public static void Copy(this DirectoryInfo source, DirectoryInfo destination)
        {
            if (!destination.Exists)
            {
                destination.Create();
            }

            // Copy all files.
            FileInfo[] files = source.GetFiles();
            foreach (FileInfo file in files)
            {
                file.CopyTo(Path.Combine(destination.FullName, file.Name));
            }

            // Process subdirectories.
            DirectoryInfo[] dirs = source.GetDirectories();
            foreach (DirectoryInfo dir in dirs)
            {
                // Get destination directory.
                string destinationDir = Path.Combine(destination.FullName, dir.Name);

                // Call CopyDirectory() recursively.
                Copy(dir, new DirectoryInfo(destinationDir));
            }
        }
    }
}