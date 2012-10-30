using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace TextProcessing
{
    public static class TextProcessor
    {
        /// <summary>
        /// apply : spaces -> space, remove non alphanumerical, to lower, stop words,steamming to text
        /// </summary>
        public static string CleanText(string Text, params char [] PersistChars)
        {
            var txt = Text;

            txt = Regex.Replace(txt, "\\s+", " ");

            txt = Regex.Replace(txt, string.Format("[^a-zA-Z0-9 {0}-]", new string(PersistChars)), "", RegexOptions.Multiline);

            txt = txt.ToLower();

            StringBuilder sb = new StringBuilder();

            foreach(var word in txt.Split(' '))
            {
                if (!string.IsNullOrEmpty(word) && !StopWords.IsStopWord(word))
                {
                    sb.Append(WordStems.GetStem(word));
                    sb.Append(" ");
                }
            }

            return sb.ToString();
            
        }
    }
}
