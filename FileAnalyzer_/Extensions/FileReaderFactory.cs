using Microsoft.Extensions.Logging;
using System;
using System.IO;

namespace FileAnalyzer_.Extensions
{
    public class FileReaderFactory
    {
        public static IFileReader Create(string path, ILoggerFactory loggerFactory)
        {
            var ext = Path.GetExtension(path).ToLowerInvariant();
            switch (ext)
            {
                case ".txt":
                    return new TxtFileReader(loggerFactory.CreateLogger<TxtFileReader>());
                case ".docx":
                    return new DocxFileReader(loggerFactory.CreateLogger<DocxFileReader>());
                case ".pdf":
                    return new PdfFileReader(loggerFactory.CreateLogger<PdfFileReader>());
                default:
                    throw new NotSupportedException($"Unsupported extension: {ext}");
            }
        }
    }
}
