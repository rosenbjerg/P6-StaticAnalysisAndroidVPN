using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using StatiskAnalyse.ResultWrappers;
using StatiskAnalyse.SearchHandling.Structure;

namespace StatiskAnalyse.SearchHandling
{
    internal class ExecutedCommandSearchHandler : IRegexSearchHandler
    {
        public string OutputName { get; } = "ExecutedProceses";
        public List<object> Process(ApkAnalysis apk, IEnumerable<Use> results)
        {
            return results.AsParallel().Select(use =>
            {
                var m = Regex.Match(use.SampleLine);
                var r = m.Groups[1].Value;
                var t = m.Groups[3].Value;
                string p;
                switch (t)
                {
                    case "Ljava/lang/String":
                        p = AnalysisTools.GetStringFromRegister(use.FoundIn, r, use.Line);
                        break;
                    case "[Ljava/lang/String":
                        p = string.Join(" ", AnalysisTools.TraceArray(use.FoundIn, use.Line, r));
                        break;
                    default:
                        throw new Exception("ExecutedCommandSearchHandler unable to process '" + use.SampleLine + "'");
                }
                return new ExecutedCommandResult
                {
                    Command = p,
                    Method = AnalysisTools.GetMethodName(use.FoundIn, use.Line),
                    Class = AnalysisTools.GetClassName(use.FoundIn),
                    SampleLine = use.SampleLine,
                    Line = use.Line,
                    File = use.File,
                    Uses = AnalysisTools.TraceMethodCall(apk, use)
                };
            }).Cast<object>().ToList();

        }

        public Regex Regex { get; } = new Regex(" *invoke-virtual {v\\d+, ([vp]\\d+)}, Ljava\\/lang\\/Runtime;->exec\\((([^ :;]+);*)\\)([^ :;]+);?", RegexOptions.Compiled);
    }

    class ExecutedCommandResult : FileResultWrapper
    {
        public string Command { get; set; }
        public string Method { get; set; }
        public string Class { get; set; }
        public string SampleLine { get; set; }
        public IEnumerable<Use> Uses { get; set; }
    }
}