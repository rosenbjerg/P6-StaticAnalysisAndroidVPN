using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using StatiskAnalyse.ResultWrappers;
using StatiskAnalyse.SearchHandling.Structure;

namespace StatiskAnalyse.SearchHandling
{
    internal class UrlRegexSearchHandler : RegexSearchHandler
    {
        public UrlRegexSearchHandler() : base(new Regex(
            "https?:\\/\\/([\\da-z\\.-]+)\\.([a-z\\.]{2,6})([\\/\\w \\.-]*)*\\/?", RegexOptions.Compiled))
        {
        }

        public override string OutputName { get; } = "Urls";

        public override List<object> Process(IEnumerable<Use> results)
        {
            return results.Cast<object>().ToList();
        }
    }
}