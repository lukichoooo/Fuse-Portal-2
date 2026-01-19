namespace FusePortal.Infrastructure.Settings.File
{
    public class FileProcessingSettings
    {
        public Dictionary<string, string> Handlers { get; set; } = [];
        public int MaxFileSizeBytes { get; set; }
    }
}
