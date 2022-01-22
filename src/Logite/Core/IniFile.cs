using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;

namespace Restless.Logite.Core
{
    /// <summary>
    /// Provides ability to read and write an ini file
    /// </summary>
    public class IniFile
    {
        /// <summary>
        /// Gets the full file name of the ini file
        /// </summary>
        public string FileName 
        { 
            get; 
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="IniFile"/> class
        /// </summary>
        /// <param name="fileName">The full file name</param>
        public IniFile(string fileName)
        {
            if (string.IsNullOrEmpty(fileName))
            {
                throw new ArgumentNullException(nameof(fileName));
            }
            if (!File.Exists(fileName))
            {
                throw new FileNotFoundException($"{fileName} not found");
            }
            FileName = fileName;
        }

        public void IniWriteValue(string section, string key, string value)
        {
            WritePrivateProfileString(section, key, value, FileName);
        }

        public string IniReadValue(string section, string key)
        {
            StringBuilder temp = new StringBuilder(255);
            int i = GetPrivateProfileString(section, key, string.Empty, temp, 255, FileName);
            return temp.ToString();
        }

        [DllImport("kernel32", CharSet = CharSet.Unicode)]
        private static extern long WritePrivateProfileString(string section, string key, string val, string filePath);
        [DllImport("kernel32", CharSet = CharSet.Unicode)]
        private static extern int GetPrivateProfileString(string section, string key, string def, StringBuilder retVal, int size, string filePath);

    }
}