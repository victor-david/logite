using Restless.Logite.Database.Core;
using System.Text.RegularExpressions;

namespace Restless.Logite.Core
{
    /// <summary>
    /// Provides static method to parse a single log line.
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
        /// <param name="domainId">The domain id</param>
        /// <param name="importFileId">The import file id</param>
        /// <param name="importFileLineNumber">The import file line number</param>
        /// <returns>An <see cref="LogEntry"/> object</returns>
        public static LogEntry ParseLine(string line, long domainId, long importFileId, long importFileLineNumber)
        {
            Match match = Regex.Match(line, RegexExpression);
            LogEntry logEntry = new(domainId, importFileId, importFileLineNumber)
            {
                RemoteAddress = match.Groups["ip"].Value,
                Status = long.Parse(match.Groups["status"].Value),
                BytesSent = long.Parse(match.Groups["bytes"].Value),
            };
            logEntry.SetRequestTime(match.Groups["date"].Value, Config.Instance.LogLineDateFormat, Config.Instance.LogLineCulture);
            logEntry.SetRequest(match.Groups["request"].Value);
            logEntry.SetReferer(match.Groups["referer"].Value);
            logEntry.SetUserAgent(match.Groups["agent"].Value);

            return logEntry;
        }
    }
}
