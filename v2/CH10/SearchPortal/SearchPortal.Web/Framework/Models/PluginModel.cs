namespace SearchPortal.Web.Framework.Models
{
    public class PluginModel
    {
        public string PackageId { get; set; }
        public string PackageVersion { get; set; }
        public string PackageDescription { get; set; }
        public bool IsInstalled { get; set; }
    }
}