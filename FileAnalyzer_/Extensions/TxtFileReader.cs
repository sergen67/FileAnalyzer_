using System;
using System.IO;
using System.Text;
using Microsoft.Extensions.Logging;

namespace FileAnalyzer_.Extensions
{

    public class TxtFileReader : IFileReader
    {
        private readonly ILogger<TxtFileReader> _logger;

        public TxtFileReader(ILogger<TxtFileReader> logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public string ReadContent(string filePath)
        {
            try
            {
                string content = File.ReadAllText(filePath, Encoding.UTF8);
                _logger.LogInformation("TXT okundu: {File}", filePath);
                return content;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "TXT okunamadı: {File}", filePath);
                throw;
            }
        }
    }
}
