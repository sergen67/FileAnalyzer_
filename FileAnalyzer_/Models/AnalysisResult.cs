using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileAnalyzer_.Models
{
    public class AnalysisResult
    {
        public int DistinctWordCount { get; }
        public List<WordCount> TopWords { get; }
        public Dictionary<string, int> PunctuationCounts { get; }
        public AnalysisResult(int distinctWordCount, List<WordCount> topWords, Dictionary<string, int> punctuationCounts)
        {
            DistinctWordCount = distinctWordCount;
            TopWords = topWords ?? new List<WordCount>();
            PunctuationCounts = punctuationCounts ?? new Dictionary<string, int>();
        }
    }
}
