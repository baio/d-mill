using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using opennlp.tools.sentdetect;
using System.IO;
using System.Text.RegularExpressions;
using opennlp.tools.tokenize;
using java.nio.charset;

namespace OpenNLP.Test
{
    [TestClass]
    public class OpenNLP
    {
        [TestMethod]
        public void SplitSentences()
        {
            var txt = File.ReadAllText(@"c:\dev\d-mill\uspe\Data\uspe-1.txt");

            txt = Regex.Replace(txt, "\\s+", " ");

            txt = Regex.Replace(txt, "\\r\\n", "");

            txt = Regex.Replace(txt, "MR.\\s+", "MR.");

            var modelStream = new java.io.FileInputStream("../../Models/en-sent.bin");

            var model = new SentenceModel(modelStream);

            var detector = new SentenceDetectorME(model);

            var sentences = detector.sentDetect(txt);

            File.WriteAllLines(@"c:\dev\d-mill\uspe\Data\uspe-sentenced.txt", sentences);
        }

        [TestMethod]
        public void Tokenize()
        {
            var modelStream = new java.io.FileInputStream("../../Models/en-token.bin");
 
    		var model = new TokenizerModel(modelStream);
 
		    var tokenizer = new TokenizerME(model);

            var txt = File.ReadAllText(@"c:\dev\d-mill\uspe\Data\uspe-sentenced.txt");

		    var tokens = tokenizer.tokenize(txt);
        }

        [TestMethod]
        public void TrainTokenizer()
        {
            var charset = Charset.forName("UTF-8");

            var lineStream = new opennlp.tools.util.PlainTextByLineStream(new java.io.FileInputStream(@"c:\dev\d-mill\uspe\Data\uspe-sentenced-train.txt"), charset);

            var sampleStream = new opennlp.tools.namefind.NameSampleDataStream(lineStream);

            var model = TokenizerME.train("en", sampleStream, true);

            var tokenizer = new TokenizerME(model);

            var tokens = tokenizer.tokenize("Hi. How are you? This is Mike.");
        }

    }
}
