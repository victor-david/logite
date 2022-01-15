using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Restless.Logite.Core
{
    /// <summary>
    /// Represents a single log entry, i.e. one line from a log file
    /// </summary>
    public class LogEntry
    {
        #region Private
        private static string[] Methods = 
        { 
            "GET", "HEAD", "POST", "PUT", "DELETE",
            "CONNECT", "OPTIONS", "TRACE", "PATCH"
        };

        private string referer;
        private string userAgent;
        #endregion

        /************************************************************************/

        #region Properties
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

        public int ByteStringLength
        {
            get;
            private set;
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
        /// Gets or sets the referer.
        /// </summary>
        public string Referer
        {
            get => referer;
            set
            {
                if (!string.IsNullOrEmpty(value) && value != "-")
                {
                    referer = value;
                }
            }
        }

        /// <summary>
        /// Gets or sets the user agent.
        /// </summary>
        public string UserAgent
        {
            get => userAgent;
            set
            {
                if (!string.IsNullOrEmpty(value) && value != "-")
                {
                    userAgent = value;
                }
            }
        }
        #endregion

        /************************************************************************/

        #region Constructor
        public LogEntry()
        {
        }
        #endregion

        /************************************************************************/

        #region Public methods

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
                foreach (string method in Methods)
                {
                    if (value.StartsWith(method))
                    {
                        Method = method;
                        break;
                    }
                }

                Request = string.IsNullOrEmpty(Method) ? value.Trim() : value.Substring(Method.Length).Trim();
                if (string.IsNullOrEmpty(Method) && Request.StartsWith(@"\x"))
                {
                    ByteStringLength = Request.Length;
                    Request = "Byte Attack";
                }
                else
                {
                    int httpPos = Request.IndexOf("HTTP/1");
                    if (httpPos != -1)
                    {
                        Request = Request.Substring(0, httpPos).Trim();
                    }
                }
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
    }
}
