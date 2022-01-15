using Restless.Logite.Database.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Restless.Logite.Core
{
    /// <summary>
    /// 
    /// </summary>
    public static class LineParser
    {
        private static string RegexExpression =
            "(?<ip>[^ ]*)" +                // ip 
             @"[^[]*\[" +                   // everything that's not a [ up to the starting [ of the date, and swallow the [
             @"(?<date>[^]]*)\]" +          // the date and swallow the ]
             "[^\"]\"" +                    // everything that's not a " up to the start of request, and swallow the quote
             "(?<request>[^\"]*)\" " +      // request including verb, swallow quote and space
             "(?<status>\\d{3}) " +         // status code, swallow space
             "(?<bytes>\\d+) " +            // bytes, swallow space
             "\"(?<referer>[^\"]*)\" " +    // referer, swallow quote and space
             "\"(?<agent>[^\"]*)";          // user agent

        /// <summary>
        /// Sets the regular expression string used to parse a log line.
        /// </summary>
        /// <param name="expression">The expression</param>
        /// <remarks>
        /// The default regular expression handles the default log format.
        /// Use this method if you have different requirements
        /// </remarks>
        public static void SetRegexExpression(string expression)
        {
            RegexExpression = expression;
        }

        /// <summary>
        /// Parses a single log line
        /// </summary>
        /// <param name="line">The line to parse</param>
        /// <returns>An <see cref="LogEntry"/> object</returns>
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
