using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace FileAnalyzer_.Reader
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
                    return (IFileReader)new DocxFileReader(loggerFactory.CreateLogger<DocxFileReader>());
                case ".pdf":
                    return (IFileReader)new PdfFileReader(loggerFactory.CreateLogger<PdfFileReader>());
                default:
                    throw new NotSupportedException($"Unsupported extension: {ext}");
            }
        }
        }
    }
//