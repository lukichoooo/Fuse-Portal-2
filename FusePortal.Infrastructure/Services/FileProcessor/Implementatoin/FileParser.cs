using System.Text;
using FusePortal.Infrastructure.Services.FileProcessor.Interfaces;
using NPOI.XWPF.UserModel;

namespace FusePortal.Infrastructure.Services.FileProcessor.Implementatoin
{
    public class FileParser : IFileParser
    {
        public Task<string> ReadDocxAsync(Stream stream)
        {
            var doc = new XWPFDocument(stream);
            var sb = new StringBuilder();

            foreach (var para in doc.Paragraphs)
                sb.AppendLine(para.ParagraphText);

            foreach (var table in doc.Tables)
            {
                foreach (var row in table.Rows)
                {
                    foreach (var cell in row.GetTableCells())
                        sb.AppendLine(cell.GetText());
                }
            }

            return Task.FromResult(sb.ToString());
        }

        public async Task<string> ReadTextAsync(Stream stream)
        {
            using var reader = new StreamReader(stream);
            return await reader.ReadToEndAsync();
        }
    }
}
