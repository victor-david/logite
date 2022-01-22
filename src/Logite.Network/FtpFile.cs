using Renci.SshNet.Sftp;
using System;
using System.Collections.Generic;
using System.Text;

namespace Restless.Logite.Network
{
    /// <summary>
    /// Represents a single ftp file.
    /// </summary>
    /// <remarks>
    /// This is a proxy class with an internal constructor to avoid having the main app
    /// depened on SSH.Net
    /// </remarks>
    public class FtpFile
    {
        /// <summary>
        /// Gets the full file name (remote)
        /// </summary>
        public string FullName
        {
            get;
        }

        /// <summary>
        /// Gets the file name portion of <see cref="FullName"/>.
        /// </summary>
        public string Name
        {
            get;
        }

        /// <summary>
        /// Gets the length of the file.
        /// </summary>
        public long Length
        {
            get;
        }

        /// <summary>
        /// Gets the last write time.
        /// </summary>
        public DateTime LastWriteTime
        {
            get;
        }

        /// <summary>
        /// Gets the last write time (UTC)
        /// </summary>
        public DateTime LastWriteTimeUtc
        {
            get;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FtpFile"/> class.
        /// </summary>
        /// <param name="file">The sftp file</param>
        internal FtpFile(SftpFile file)
        {
            if (file == null)
            {
                throw new ArgumentNullException(nameof(file));
            }
            FullName = file.FullName;
            Name = file.Name;
            Length = file.Length;
            LastWriteTime = file.LastWriteTime;
            LastWriteTimeUtc = file.LastWriteTimeUtc;
        }
    }
}