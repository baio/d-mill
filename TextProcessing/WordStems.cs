using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TextProcessing
{
    public static class WordStems
    {
        public static string GetStem(string Word)
        {

            var stemmer = new PorterStemmerAlgorithm.PorterStemmer();

            return stemmer.stemTerm(Word);


        }

        public static bool CompareStems(string Word1, string Word2)
        {
            return GetStem(Word1) == GetStem(Word2);
        }
    }
}
