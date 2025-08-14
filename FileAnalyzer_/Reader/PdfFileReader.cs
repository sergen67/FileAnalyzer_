using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UglyToad.PdfPig;

namespace FileAnalyzer_.Reader
{//Pdf dosyalarını okuyan sınıf
    public class PdfFileReader : IFileReader
    {
        private readonly ILogger<PdfFileReader> _logger;
        public PdfFileReader(ILogger<PdfFileReader> logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }
        public string ReadContent(string filePath)
        {
            try
            {
                var sb = new StringBuilder();
                using(var pdf = PdfDocument.Open(filePath))
                {
                    foreach (var page in pdf.GetPages())
                    {
                        sb.AppendLine(page.Text);
                    }
                }
                _logger.LogInformation("PDF okundu: {File}", filePath);
                return sb.ToString();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "PDF okunamadı: {File}", filePath);
                throw; 
            }
        }
    }
}
