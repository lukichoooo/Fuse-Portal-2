namespace FusePortal.Application.Interfaces.Auth
{
    public interface IEncryptor
    {
        public string Encrypt(string raw);
        public string Decrypt(string encrypted);
    }
}
