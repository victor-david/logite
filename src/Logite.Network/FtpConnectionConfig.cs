using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Restless.Logite.Network
{
    /// <summary>
    /// Represents the data needed to connection and download remote log files.
    /// </summary>
    public class FtpConnectionConfig
    {
        #region Properties
        /// <summary>
        /// Gets the host
        /// </summary>
        public string Host
        {
            get;
        }

        /// <summary>
        /// Gets the user name
        /// </summary>
        public string UserName
        {
            get;
        }

        /// <summary>
        /// Gets the path to the local key file
        /// </summary>
        public string KeyFile
        {
            get;
        }

        /// <summary>
        /// Gets the remote log directory
        /// </summary>
        public string RemoteLogDirectory
        {
            get;
        }

        /// <summary>
        /// Gets the local log directory
        /// </summary>
        public string LocalLogDirectory
        {
            get;
        }
        #endregion

        /************************************************************************/

        #region Constructor
        /// <summary>
        /// Initializes a new instance of the <see cref="FtpConnectionConfig"/> class.
        /// </summary>
        /// <param name="host">The host</param>
        /// <param name="userName">The user name</param>
        /// <param name="keyFile">Full path to the key file</param>
        /// <param name="remoteDirectory">The remote directory to obtain log files.</param>
        /// <param name="localDirectory">The local directory to download to.</param>
        public FtpConnectionConfig(string host, string userName, string keyFile, string remoteDirectory, string localDirectory)
        {
            Host = GetValidatedString(host, nameof(Host));
            UserName = GetValidatedString(userName, nameof(UserName));
            KeyFile = GetValidatedString(keyFile, nameof(KeyFile));
            RemoteLogDirectory = GetValidatedString(remoteDirectory, nameof(RemoteLogDirectory));
            LocalLogDirectory = GetValidatedString(localDirectory, nameof(LocalLogDirectory));
            if (!File.Exists(KeyFile))
            {
                throw new Exception($"Key file {KeyFile} does not exist");
            }

            if (!Directory.Exists(LocalLogDirectory))
            {
                throw new Exception($"Local download directory {LocalLogDirectory} does not exist");
            }
        }
        #endregion

        /************************************************************************/

        #region Private methods
        private string GetValidatedString(string value, string name)
        {
            if (string.IsNullOrEmpty(value))
            {
                throw new ArgumentException($"Value of {name} cannot be empty");
            }
            return value;
        }
        #endregion
    }
}
