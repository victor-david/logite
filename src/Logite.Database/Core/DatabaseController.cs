using Restless.Logite.Database.Tables;
using Restless.Toolkit.Core.Database.SQLite;
using System;
using System.IO;

namespace Restless.Logite.Database.Core
{
    /// <summary>
    /// A singleton class to provide high level database management via SQLite
    /// </summary>
    public sealed class DatabaseController : DatabaseControllerBase
    {
        #region Private
        private const string PrivateSchema = "private";
        #endregion

        /************************************************************************/

        #region Public
#if DEBUG
        public const string DefaultDataSet = "SETD-V100-FEF4-4C64-9073";
#else
        public const string DefaultDataSet = "SETR-V100-FEF4-4C64-9073";
#endif
        /// <summary>
        /// Gets the default alias for the main (system default) data
        /// </summary>
        public const string MainFileAlias = "Main data";

        /// <summary>
        /// Gets the file id for the main (system default) data
        /// </summary>
        public const string MainFileId    = "MAIN0-45B9C548-DA20-407D-85E1-A1DE1C0553E6";

        /// <summary>
        /// Gets the name for the attached data schema. This schema holds all the main tables.
        /// </summary>
        public const string MainAppSchemaName = "logite";

        /// <summary>
        /// Gets the name for the attached memory only schema
        /// </summary>
        public const string MemorySchemaName = "mem";

        /// <summary>
        /// Gets the alias for a memory file.
        /// </summary>
        public const string MemoryFileAlias = "Memory only data";

        /// <summary>
        /// Gets the database root location. If the user changes the location,
        /// the new value is not used and this property does not change, until
        /// the application is restarted.
        /// </summary>
        public string DatabaseRoot
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the alias of the current database.
        /// </summary>
        public string MainDatabaseAlias
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the id of the current database.
        /// </summary>
        public string MainDatabaseId
        {
            get;
            private set;
        }
        #endregion

        /************************************************************************/

        #region Singleton access and constructor
        /// <summary>
        /// Gets the singleton instance of this class
        /// </summary>
        public static DatabaseController Instance { get; } = new DatabaseController();

        /// <summary>
        /// Constructor (private)
        /// </summary>
        private DatabaseController() : base()
        {
        }
        #endregion

        /************************************************************************/

        #region Public methods
        /// <summary>
        /// Initializes the database controller by creating and registering 
        /// all of the tables for the application.
        /// </summary>
        /// <param name="databaseRoot">The database root folder.</param>
        public void Init(string databaseRoot)
        {
            if (string.IsNullOrEmpty(databaseRoot)) throw new ArgumentNullException(nameof(databaseRoot));
            DatabaseRoot = databaseRoot;
            CreateAndOpen(MemoryDatabase);
            AttachSchemaDatabase();
            AttachMainDatabase(MainFileAlias, MainFileId);
        }

        /// <summary>
        /// Attaches the main finance database
        /// </summary>
        /// <param name="databaseFileId">The database name (not path)</param>
        public void AttachMainDatabase(string alias, string databaseFileId)
        {
            if (string.IsNullOrEmpty(databaseFileId)) throw new ArgumentNullException(nameof(databaseFileId));

            string fullFileName =  GetFileNameFromId(databaseFileId);

            Attach(MainAppSchemaName, fullFileName, () =>
            {
                CreateAndRegisterTable<AttackTable>();
                CreateAndRegisterTable<ConfigTable>();
                CreateAndRegisterTable<DomainTable>();
                CreateAndRegisterTable<LogEntryTable>();
                CreateAndRegisterTable<ImportFileTable>();
                CreateAndRegisterTable<IpAddressTable>();
                CreateAndRegisterTable<MethodTable>();
                CreateAndRegisterTable<RefererTable>();
                CreateAndRegisterTable<RequestTable>();
                CreateAndRegisterTable<StatusTable>();
                CreateAndRegisterTable<UserAgentTable>();
                TableRegistrationComplete(MainAppSchemaName);
                MainDatabaseAlias = alias;
                MainDatabaseId = databaseFileId;
            });
        }

        /// <summary>
        /// Detaches the main finance database.
        /// </summary>
        public void DetachMainDatabase()
        {
            Detach(MainAppSchemaName);
            MainDatabaseAlias = MainDatabaseId = null;
        }

        ///// <summary>
        /// <summary>
        /// Gets the full file name from the file id.
        /// </summary>
        /// <param name="fileId">The file id</param>
        /// <returns><paramref name="fileId"/> if memory file; otherwise, prepends database root and data set.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="fileId"/> is null or empty</exception>
        public string GetFileNameFromId(string fileId)
        {
            if (string.IsNullOrEmpty(fileId))
            {
                throw new ArgumentNullException(nameof(fileId));
            }

            if (fileId != MemoryDatabase)
            {
                return Path.Combine(DatabaseRoot, DefaultDataSet, fileId);
            }
            return fileId;
        }
        #endregion

        /************************************************************************/

        #region Private methods

        private bool TryAttach(string fullFileName)
        {
            try 
            {
                Attach(PrivateSchema, fullFileName, () => { });
                return true;
            }
            catch
            {
                return false;
            }
            finally
            {
                Detach(PrivateSchema);
            }
        }

        private void AttachSchemaDatabase()
        {
            Attach(MemorySchemaName, MemoryDatabase, () =>
            {
                CreateAndRegisterTable<SchemaTable>();
                TableRegistrationComplete(MemorySchemaName);
            });
        }
        #endregion
    }
}