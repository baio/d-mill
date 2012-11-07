using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using TextProcessing;

namespace uspe
{
    class Program
    {
        static void Main(string[] args)
        {
            //PrepareData.Prepare();
            //CountriesMentionList.Proccess();
            //CountriesMentionList.Count();

            //var txt = TextProcessor.JoinFiles("../../Data/uspe-{0}.txt", 3, "../../Data/Processed/uspe-1-joint.txt");
            var txt = TextProcessor.JoinFiles("../../Data/uspe-{0}.txt", 1, "../../Data/Processed/uspe-1-joint.txt");

            txt = Regex.Replace(txt, "MR.", "Mr.");

            var sent = TextProcessor.SplitSentences(txt);

            File.WriteAllLines("../../Data/Processed/uspe-sentenced-2.txt", sent);

            //txt = File.ReadAllText("../../Data/Processed/uspe-sentenced-2.txt");

            //var chunks = TextProcessor.GetChunks(sent).ToArray();

            var discuss = TextProcessor.AsDisscuss(sent);
        }
    }
}
