using System;

namespace FileAnalyzer_.Models
{
    public class WordCount
    {
        public string Word { get; }
        public int Count { get; }
        public WordCount(string word, int count) { Word = word; Count = count; }
        public override string ToString() => $"{Word}: {Count}";
    }
}
