using FusePortal.Infrastructure.Services.FileProcessor.Exceptions;
using FusePortal.Infrastructure.Services.FileProcessor.Interfaces;
using OpenCvSharp;
using Sdcb.PaddleOCR;
using Sdcb.PaddleInference;
using Sdcb.PaddleOCR.Models;
using Sdcb.PaddleOCR.Models.Local;

namespace FusePortal.Infrastructure.Services.FileProcessor.Implementatoin
{
    public class PaddleOcrService : IOcrService
    {
        private readonly PaddleOcrAll _ocr;

        public PaddleOcrService()
        {
            FullOcrModel model = LocalFullModels.EnglishV4;
            _ocr = new PaddleOcrAll(model, PaddleDevice.Mkldnn())
            {
                AllowRotateDetection = true,
                Enable180Classification = false,
            };
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
            catch (Exception)
            {
                throw new UnsupportedFileParseException("Unable to parse file");
            }
        }

        public Task<string> FallbackOcrAsync(Stream fileStream)
        {
            try
            {
                return ProcessAsync(fileStream);
            }
            catch (UnsupportedFileParseException)
            {
                throw;
            }
        }
    }
}
