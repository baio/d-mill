using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace TextProcessing
{
    public static class TextClean
    {
        public static string Clean(string Text, TextCleanType CleanType)
        {
            if ((CleanType & TextCleanType.RemoveExtraSpaces) == TextCleanType.RemoveExtraSpaces)
            {
                   Text = Regex.Replace(Text, "\\s+"," ").Trim();
            }
            else if((CleanType & TextCleanType.RemoveExtraSpaces) == TextCleanType.RemoveNewLines)
            {
                Text = Regex.Replace(Text, "\\r\\n","").Trim();
            }

            return Text;
        }
    }

    [Flags]
    public enum TextCleanType
    { 
        RemoveExtraSpaces = 0x1,

        RemoveNewLines = 0x10
    }
}
