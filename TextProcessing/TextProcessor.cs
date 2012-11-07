using opennlp.tools.chunker;
using opennlp.tools.postag;
using opennlp.tools.sentdetect;
using opennlp.tools.tokenize;
using System;
using System.Collections.Generic;
using System.IO;
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

        public static string JoinFiles(string FileNameTemplate, int FilesCount, string OutputFileName = null, int FilesCountStart = 1, string Separator = "\r\n", TextCleanType CleanBeforeJoin =  TextCleanType.RemoveExtraSpaces | TextCleanType.RemoveNewLines)
        {
            string txt = null;

            var list = new List<string>();

            for (var i = FilesCountStart; i < FilesCountStart + FilesCount; i++)
            {
                txt = File.ReadAllText(string.Format(FileNameTemplate, i));

                txt = TextClean.Clean(txt, CleanBeforeJoin);

                list.Add(txt);
            }

            txt = string.Join(Separator, list);

            if (!string.IsNullOrEmpty(OutputFileName))
                File.WriteAllText(OutputFileName, txt);

            return txt;
        }

        public static string [] SplitSentences(string Text)
        {
            var modelStream = new java.io.ByteArrayInputStream(Resource.en_sent);

            var model = new SentenceModel(modelStream);

            var detector = new SentenceDetectorME(model);

            return detector.sentDetect(Text);
        }

        public static IEnumerable<IEnumerable<ChunkItem>> GetChunks(IEnumerable<string> Sentences)
        {
            var posModelStream = new java.io.ByteArrayInputStream(Resource.en_pos_maxent);//new java.io.FileInputStream(@"C:\dev\d-mill\TextProcessing\OpenNLP\Models\en-pos-maxent.bin");

            var posModel = new POSModel(posModelStream);

            var pos = new POSTaggerME(posModel);

            var modelStream = new java.io.ByteArrayInputStream(Resource.en_token); //java.io.FileInputStream(@"C:\dev\d-mill\TextProcessing\OpenNLP\Models\en-token.bin");

            var model = new TokenizerModel(modelStream);

            var tokenizer = new TokenizerME(model);

            var chunkerModelStream = new java.io.ByteArrayInputStream(Resource.en_chunker);

            var chunkerModel = new ChunkerModel(chunkerModelStream);

            var chunker = new ChunkerME(chunkerModel);

            return Sentences.Select(p => {

                var tokens = tokenizer.tokenize(p);

                var tags = pos.tag(tokens);

                var chunks = chunker.chunk(tokens, tags);

                var res = new List<ChunkItem>();

                for (var i = 0; i < chunks.Length; i++)
                {
                    res.Add(new ChunkItem { token = tokens[i], tag = tags[i], chunk = chunks[i] });
                }

                return res;
            });            
        }

        public static IEnumerable<KeyValuePair<string, string>> AsDisscuss(IEnumerable<string> Sentences)
        {
            var res = new List<KeyValuePair<string, string>>();

            var chunks = GetChunks(Sentences).ToArray();

            string person = null;

            for (var i = 0; i < Sentences.Count(); i++)
            {
                var sentence = Sentences.ElementAt(i);

                var chunk = chunks.ElementAt(i);

                if (chunk.First().chunk == "B-NP" && chunk.FirstOrDefault(p => p.token == ":") != null)
                {
                    var personChunks = chunk.TakeWhile(p=>p.token != ":");


                    //1. started chunks is NOUN (name) till : (semicolmn)
                    if (personChunks.Count() == 1 || personChunks.Skip(1).Where(p => p.chunk != "I-NP").Count() == 0)
                    {
                        person = string.Join(" ", personChunks.Select(p => p.token));

                        sentence = string.Join(" ", personChunks.SkipWhile(p => p.token != ":").Select(p=>p.token));
                    }
                }

                res.Add(new KeyValuePair<string, string>(person, sentence));    
            }

            return res;
        }

        public class ChunkItem
        {
            public string token;

            public string tag;

            public string chunk;
        }
    }
}
