using FusePortal.Api.Settings;
using FusePortal.Application.UseCases.Content.Files;
using FusePortal.Application.UseCases.Content.Files.Commands.UploadFiles;
using FusePortal.Application.UseCases.Content.Files.Queries.GetFileById;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace FusePortal.Api.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    [ApiController]
    public class FileController(
            ISender sender,
            IOptions<ControllerSettings> options) : ControllerBase
    {
        private readonly ISender _sender = sender;

        [HttpPost("upload")]
        public async Task<ActionResult<List<Guid>>> UploadFilesAsync(
                [FromForm] IFormFileCollection Files
                )
            => Ok(await _sender.Send(new UploadFilesCommand(
                        await Files.ToFileUpload())));


        [HttpGet("{fileId}")]
        public async Task<ActionResult<FileDto>> GetFileByIdAsync(
                [FromRoute] Guid fileId
                )
            => Ok(_sender.Send(new GetFileByIdQuery(fileId)));

    }


    // Helper
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

