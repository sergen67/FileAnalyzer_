using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using FileAnalyzer_.Models;


namespace FileAnalyzer_.Reader
{
    public static class TextAnalyzer
    {
        static readonly HashSet<string> StopWords = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
        {
            "ve","ile","ama","ancak","fakat","de","da","ki","ise","veya","yahut",
            "the","a","an","and","or","but","if","in","on","at","to","of","for","with"
        };

        public static AnalysisResult Analyze(string text)
        {
            if (text == null) text = string.Empty;

            var punctuationCounts = new Dictionary<string, int>();
            foreach (Match m in Regex.Matches(text, @"[.,!?\-;:()""'…]"))
            {
                var key = m.Value;
                if (punctuationCounts.ContainsKey(key)) punctuationCounts[key]++;
                else punctuationCounts[key] = 1;
            }


            var words = new List<string>();
            foreach (Match m in Regex.Matches(text.ToLowerInvariant(), @"\b[^\W\d_]+\b"))
            {
                var w = m.Value;
                if (!StopWords.Contains(w)) words.Add(w);
            }
          

            int MinFreq = 2;
            var freqList = words
                .GroupBy(w => w)
                .Where(g => g.Count() >= MinFreq)
                .Select(g => new WordCount(g.Key, g.Count()))
                .OrderByDescending(x => x.Count)
                .ThenBy(x => x.Word)
                .ToList();

            return new AnalysisResult(
                distinctWordCount: freqList.Count,
                topWords: freqList,
                punctuationCounts: punctuationCounts
            );
        }
    }
}
