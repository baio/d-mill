using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TextProcessing
{
    public static class StopWords
    {
        public static bool IsStopWord(string Word)
        {
            return Resource.common_english_words.Contains("," + Word + ",");
        }
    }
}
