using FusePortal.Application.Interfaces.Services.File;

namespace FusePortal.Api.Controllers.Extensions
{
    public static class FormFileCollectionExtensions
    {
        public static async Task<List<FileUpload>> ToFileUpload(
                this IFormFileCollection formFiles)
        {
            var result = new List<FileUpload>(formFiles.Count);
            foreach (var file in formFiles)
            {
                if (file.Length == 0)
                    continue;
                result.Add(new FileUpload(file.FileName, file.OpenReadStream()));
            }
            return result;
        }
    }
}
