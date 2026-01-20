namespace FusePortal.Application.Interfaces.Services.File
{
    public record FileUpload(
            string Name,
            Stream Stream
            );
}
