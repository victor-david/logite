using Restless.Logite.Database.Tables;
using System;
using System.Collections.Generic;
using System.Globalization;

namespace Restless.Logite.Database.Core
{
    /// <summary>
    /// Represents a single log entry, i.e. one line from a log file
    /// </summary>
    public class LogEntry
    {
        #region Private
        private const string HttpVersionZero = "0.0";
        #endregion

        /************************************************************************/

        #region Constants
        /// <summary>
        /// Used when a request, referer, or user agent is empty.
        /// </summary>
        public const string EmptyEntryPlaceholder = "--";
        #endregion

        /************************************************************************/

        #region Properties
        /// <summary>
        /// Gets the domain id for this log entry
        /// </summary>
        public long DomainId
        {
            get;
        }

        /// <summary>
        /// Gets or sets the import file id for this entry.
        /// </summary>
        public long ImportFileId
        {
            get;
        }

        /// <summary>
        /// Gets or sets the import file line number for this entry.
        /// </summary>
        public long ImportFileLineNumber
        {
            get;
        }

        /// <summary>
        /// Gets or sets the remote address.
        /// </summary>
        public string RemoteAddress
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the remote user.
        /// </summary>
        public string RemoteUser
        {
            get;
            set;
        }

        /// <summary>
        /// Gets the request time.
        /// To set the request time, call <see cref="SetRequestTime(string, string, string)"/>
        /// </summary>
        public DateTime RequestTime
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the http method.
        /// To set the method, call <see cref="SetRequest(string)"/>
        /// </summary>
        public string Method
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the request.
        /// To set the request, call <see cref="SetRequest(string)"/>
        /// </summary>
        public string Request
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the http version, i.e "1.0", "1.1"
        /// To set, call <see cref="SetRequest(string)"/>
        /// </summary>
        public string HttpVersion
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the attack vector list, empty if no attacks.
        /// </summary>
        public List<AttackVector> Attacks
        {
            get;
        }

        /// <summary>
        /// Gets or sets the request status.
        /// </summary>
        public long Status
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the bytes sent.
        /// </summary>
        public long BytesSent
        {
            get;
            set;
        }

        /// <summary>
        /// Gets the referer.
        /// To set, call <see cref="SetReferer(string)"/>
        /// </summary>
        public string Referer
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the user agent.
        /// To set, call <see cref="SetUserAgent(string)"/>
        /// </summary>
        public string UserAgent
        {
            get;
            private set;
        }
        #endregion

        /************************************************************************/

        #region Constructor
        /// <summary>
        /// Creates a new instance of the <see cref="LogEntry"/> class.
        /// </summary>
        /// <param name="domainId">Domain id of this entry.</param>
        /// <param name="importFileId">The import file id</param>
        /// <param name="importFileLineNumber">The import file line number</param>
        public LogEntry(long domainId, long importFileId, long importFileLineNumber)
        {
            DomainId = domainId;
            ImportFileId = importFileId;
            ImportFileLineNumber = importFileLineNumber;
            HttpVersion = HttpVersionZero;
            Request = Referer = UserAgent = EmptyEntryPlaceholder;
            Attacks = new List<AttackVector>();
        }
        #endregion

        /************************************************************************/

        #region Public methods
        /// <summary>
        /// Sets the request time from the parsed string value.
        /// </summary>
        /// <param name="value">The value from which to extract the request time.</param>
        /// <param name="format">The format to use.</param>
        /// <param name="culture">The culture identifier.</param>
        public void SetRequestTime(string value, string format, string culture)
        {
            // Date example:   14/Jan/2022:09:37:43 -0600
            // Default format: dd/MMM/yyyy:HH:mm:ss zzz
            RequestTime = DateTime.ParseExact(value, format, new CultureInfo(culture));
        }

        /// <summary>
        /// Sets the request from the parsed value.
        /// </summary>
        /// <param name="value">The complete request string</param>
        /// <remarks>
        /// This method expects that the entire request string is passed, i.e. "GET /user/test HTTP/1.1"
        /// It extracts the method and the request, populating <see cref="Method"/> and <see cref="Request"/>.
        /// </remarks>
        public void SetRequest(string value)
        {
            if (!string.IsNullOrEmpty(value))
            {
                Method = DatabaseController.Instance.GetTable<MethodTable>().GetMethodName(value);

                string request = string.IsNullOrEmpty(Method) ? value.Trim() : value.Substring(Method.Length).Trim();
                if (string.IsNullOrEmpty(Method) || IsAttackString(request))
                {
                    Attacks.Add(new AttackVector(AttackVectorType.Request, request));
                    request = AttackVector.RequestAttackName;
                }
                else
                {
                    int httpPos = request.IndexOf("HTTP/1");
                    if (httpPos != -1)
                    {
                        string[] http = request.Substring(httpPos).Split(new char[] { '/' }, StringSplitOptions.RemoveEmptyEntries);
                        request = request.Substring(0, httpPos).Trim();
                        if (http.Length > 1)
                        {
                            HttpVersion = http[1];
                        }
                    }
                }
                Request = Uri.UnescapeDataString(request);
            }
            else
            {
                Request = EmptyEntryPlaceholder;
            }
        }

        /// <summary>
        /// Sets the referer.
        /// </summary>
        /// <param name="value"></param>
        public void SetReferer(string value)
        {
            if (IsAttackString(value))
            {
                Attacks.Add(new AttackVector(AttackVectorType.Referer, value));
                Referer = AttackVector.RefererAttackName;
            }
            else
            {
                Referer = GetCleanValue(value);
            }
        }

        /// <summary>
        /// Sets the user agent.
        /// </summary>
        /// <param name="value"></param>
        public void SetUserAgent(string value)
        {
            if (IsAttackString(value))
            {
                Attacks.Add(new AttackVector(AttackVectorType.UserAgent, value));
                UserAgent = AttackVector.AgentAttackName;
            }
            else
            {
                UserAgent = GetCleanValue(value);
            }
        }

        /// <summary>
        /// Returns a string representation of this object.
        /// </summary>
        /// <returns>A string</returns>
        public override string ToString()
        {
            return $"{RemoteAddress} Time:{RequestTime} Method:{Method} Request:{Request} Status:{Status} Bytes:{BytesSent}";
        }
        #endregion

        /************************************************************************/

        #region Private methods
        private bool IsAttackString(string value)
        {
            return
                !string.IsNullOrEmpty(value) &&
                (value.Contains(@"\x") || value.Contains("'") || value.Contains("\""));
        }

        private string GetCleanValue(string value)
        {
            if (string.IsNullOrEmpty(value) || value == "-")
            {
                return EmptyEntryPlaceholder;
            }
            return value;
        }
        #endregion
    }
}