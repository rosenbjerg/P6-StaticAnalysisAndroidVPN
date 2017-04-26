using System.Collections.Generic;
using Newtonsoft.Json;

namespace StatiskAnalyse
{
    public class SearchResult
    {
        public string Pattern { get; set; }
        public int UseCount => Uses.Count;
        public List<Use> Uses { get; } = new List<Use>();

        public override string ToString()
        {
            return $"{Pattern} : {Uses.Count} uses";
        }

        public class Use
        {
            [JsonIgnore]
            public ClassFile FoundIn { get; }

            public string File => FoundIn.FilePath;
            public int Index { get; }
            public int Line { get; set; }
            
            public Use(ClassFile cf, int line, int col, string ll)
            {
                FoundIn = cf;
                Line = line;
                Index = col;
                SampleLine = ll;
            }

            public string SampleLine { get; set; }
        }
    }

}