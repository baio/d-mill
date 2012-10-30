using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace uspe
{
    static class PrepareData
    {
        internal static void Prepare()
        { 
            string fullTxt = "";

            for(int i = 1; i <=3 ; i++)
            {
                var txt = File.ReadAllText(string.Format("../../Data/uspe-{0}.txt", i));

                txt = TextProcessing.TextProcessor.CleanText(txt, ':');

                fullTxt += txt;

                fullTxt += "\r\n===\r\n";
            }

            File.WriteAllText("../../Data/uspe-all.txt", fullTxt);            
        }
    }
}
