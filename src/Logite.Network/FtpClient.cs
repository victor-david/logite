using Renci.SshNet;
using Renci.SshNet.Sftp;
using System;
using System.IO;
using System.Threading.Tasks;

namespace Restless.Logite.Network
{
    /// <summary>
    /// Represents an ftp client for obtaing remote log files
    /// </summary>
    public class FtpClient
    {
        #region Properties
        /// <summary>
        /// Gets the configuration used for this client
        /// </summary>
        public FtpConnectionConfig Config
        {
            get;
        }
        #endregion

        /************************************************************************/

        #region Constructor
        /// <summary>
        /// Initializes a new instance of the <see cref="FtpClient"/> class.
        /// </summary>
        /// <param name="config">The configuration</param>
        public FtpClient(FtpConnectionConfig config)
        {
            Config = config ?? throw new ArgumentNullException(nameof(config));
        }
        #endregion

        /************************************************************************/

        #region Public methods
        /// <summary>
        /// Asynchronously downloads files
        /// </summary>
        /// <param name="callback">
        /// A callback that receives the <see cref="FtpFile"/>.
        /// If the callback returns false, the file is not downloaded
        /// </param>
        /// <returns></returns>
        public async Task DownloadLogFilesAsync(Predicate<FtpFile> callback)
        {
            await Task.Run(() =>
            {
                ConnectionInfo connectionInfo = new ConnectionInfo(
                    Config.Host, 
                    Config.UserName, 
                    new PrivateKeyAuthenticationMethod(Config.UserName, new PrivateKeyFile(Config.KeyFile)));

                using (SftpClient client = new SftpClient(connectionInfo))
                {
                    client.Connect();
                    foreach (SftpFile file in client.ListDirectory(Config.RemoteLogDirectory))
                    {
                        if (file.IsRegularFile && callback(new FtpFile(file)))
                        {
                            string localFileName = Path.Combine(Config.LocalLogDirectory, file.Name);
                            using (var fs = new FileStream(localFileName, FileMode.Create))
                            {
                                client.DownloadFile(file.FullName, fs);
                            }
                        }
                    }
                    client.Disconnect();
                }
            });
        }
        #endregion
    }
}