using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace uspe
{
    static class CountriesMentionList
    {
        internal static void Proccess()
        {
            var connectionString = ConfigurationManager.AppSettings["MONGO_URI"];

            var hostName = Regex.Replace(connectionString, "^(.*)/(.*)$", "$1");
            var dbName = Regex.Replace(connectionString, "^(.*)/(.*)$", "$2");

            var server = MongoServer.Create(hostName);
            server.Connect();
            var db = server.GetDatabase(dbName);

            var countries = db.GetCollection<Country>("countries").FindAll();

            var countryAlias = countries.Select(v => new { name = v._id, alias = v.alias.Union(new [] {v.name}).Select(p=>TextProcessing.WordStems.GetStem(p))}).ToArray();

            var txt = File.ReadAllText("../../Data/uspe-all.txt");

            var posCountryPairs = new Dictionary<int, string>();

            var wordPos = 0;

            foreach (var word in txt.Split(' '))
            {
                var found = countryAlias.FirstOrDefault(p => p.alias.Contains(word));

                if (found != null)
                {
                    posCountryPairs.Add(wordPos, found.name);
                }

                wordPos++;
            }

            File.WriteAllText("../../Data/uspe-country-mention-raw.txt", string.Join("\r\n", posCountryPairs.Select(p=>string.Format("{0};{1}", p.Value, p.Key))));
        }

        internal static void Count()
        {
            Dictionary<string, int> countryCntPairs = new Dictionary<string, int>();

            foreach (var line in File.ReadAllLines("../../Data/uspe-country-mention-raw.txt"))
            { 
                var pair = line.Split(';');
                var ctry = pair[0];

                if (!countryCntPairs.Keys.Contains(ctry))
                {
                    countryCntPairs.Add(ctry, 1);
                }
                else
                {
                    countryCntPairs[ctry] = countryCntPairs[ctry] + 1;
                }
            }

            File.WriteAllText("../../Data/uspe-country-mention.txt", string.Join("\r\n", countryCntPairs.OrderBy(p=>p.Key).Select(p => string.Format("{0};{1}", p.Key, p.Value))));

        }

        internal class Country
        {
            public string _id { get; set; }

            public string name { get; set; }

            public string[] alias { get; set; }
        }
    }
}
