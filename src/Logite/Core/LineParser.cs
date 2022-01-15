using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Restless.Logite.Core
{
    public static class LineParser
    {
        private const string RegexExpression =
            "(?<ip>[^ ]*)" +                // ip 
             @"[^[]*\[" +                   // everything that's not a [ up to the starting [ of the date, and swallow the [
             @"(?<date>[^]]*)\]" +          // the date and swallow the ]
             "[^\"]\"" +                    // everything that's not a " up to the start of request, and swallow the quote
             "(?<request>[^\"]*)\" " +      // request including verb, swallow quote and space
             "(?<status>\\d{3}) " +         // status code, swallow space
             "(?<bytes>\\d+) " +            // bytes, swallow space
             "\"(?<referer>[^\"]*)\" " +    // referer, swallow quote and space
             "\"(?<agent>[^\"]*)";          // user agent

        public static LogEntry ParseLine(string line)
        {
            Match match = Regex.Match(line, RegexExpression);
            LogEntry parsedLine = new()
            {
                RemoteAddress = match.Groups["ip"].Value,
                Status = long.Parse(match.Groups["status"].Value),
                BytesSent = long.Parse(match.Groups["bytes"].Value),
                Referer = match.Groups["referer"].Value,
                UserAgent = match.Groups["agent"].Value
            };
            parsedLine.SetRequestTime(match.Groups["date"].Value, Config.Instance.LogLineDateFormat, Config.Instance.LogLineCulture);
            parsedLine.SetRequest(match.Groups["request"].Value);
            return parsedLine;
        }
    }
}
