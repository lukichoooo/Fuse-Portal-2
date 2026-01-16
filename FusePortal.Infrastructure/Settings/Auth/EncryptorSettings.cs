namespace FusePortal.Infrastructure.Settings.Auth
{
    public class EncryptorSettings
    {
        public byte[] Key { get; set; } = null!;
        public byte[] IV { get; set; } = null!;
    }
}
