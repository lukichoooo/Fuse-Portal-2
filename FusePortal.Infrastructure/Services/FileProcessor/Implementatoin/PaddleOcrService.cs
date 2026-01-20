using FusePortal.Infrastructure.Services.FileProcessor.Exceptions;
using FusePortal.Infrastructure.Services.FileProcessor.Interfaces;
using OpenCvSharp;
using Sdcb.PaddleOCR;
using Sdcb.PaddleInference;
using Sdcb.PaddleOCR.Models;
using Sdcb.PaddleOCR.Models.Local;
using Microsoft.Extensions.Logging;

namespace FusePortal.Infrastructure.Services.FileProcessor.Implementatoin
{
    public class PaddleOcrService : IOcrService
    {
        private readonly PaddleOcrAll _ocr;
        private readonly ILogger<IOcrService> _logger;

        public PaddleOcrService(ILogger<IOcrService> logger)
        {
            FullOcrModel model = LocalFullModels.EnglishV4;
            _ocr = new PaddleOcrAll(model, PaddleDevice.Mkldnn())
            {
                AllowRotateDetection = true,
                Enable180Classification = false,
            };
            _logger = logger;
        }

        public async Task<string> ProcessAsync(Stream fileStream)
        {
            try
            {
                byte[] data;
                await using (var ms = new MemoryStream())
                {
                    await fileStream.CopyToAsync(ms);
                    data = ms.ToArray();
                }

                using var img = Cv2.ImDecode(data, ImreadModes.Color);
                if (img.Empty())
                    throw new UnsupportedFileParseException("Image could not be decoded");

                var result = _ocr.Run(img);

                return result.Text;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Unable to parse file : {fileStream}, exception={ex.Message}");
                _logger.LogInformation($"OpenCvSharp Version : {Cv2.GetVersionString()}");
                throw new UnsupportedFileParseException("Unable to parse file");
            }
        }

        public async Task<string> FallbackOcrAsync(Stream fileStream)
        {
            return await ProcessAsync(fileStream);
        }
    }
}
