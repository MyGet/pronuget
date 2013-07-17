namespace SearchPortal.Web.ViewModels
{
    public class PluginViewModel
    {
        public string PackageId { get; set; }
        public string PackageVersion { get; set; }
        public string PackageDescription { get; set; }
        public bool IsInstalled { get; set; }
    }
}