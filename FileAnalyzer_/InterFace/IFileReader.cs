using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileAnalyzer_
{
    
    public interface IFileReader
    {
        string ReadContent(string filePath);
    }
}
