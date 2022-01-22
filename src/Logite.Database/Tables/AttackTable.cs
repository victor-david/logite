using Restless.Logite.Database.Core;
using Restless.Toolkit.Core.Database.SQLite;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace Restless.Logite.Database.Tables
{
    public class AttackTable : Core.ApplicationTableBase
    {
        #region Public properties
        /// <summary>
        /// Provides static definitions for table properties such as column names and relation names.
        /// </summary>
        public static class Defs
        {
            /// <summary>
            /// Specifies the name of this table.
            /// </summary>
            public const string TableName = "attack";

            /// <summary>
            /// Provides static column names for this table.
            /// </summary>
            public static class Columns
            {
                /// <summary>
                /// The name of the id column. This is the table's primary key.
                /// </summary>
                public const string Id = DefaultPrimaryKeyName;

                /// <summary>
                /// The attack type.
                /// </summary>
                public const string AttackType = "attacktype";

                /// <summary>
                /// The attack string
                /// </summary>
                public const string AttackString = "attackstr";
            }

            /// <summary>
            /// Provides static relation names.
            /// </summary>
            public static class Relations
            {
                /// <summary>
                /// The name of the relation that relates the <see cref="ImportFileTable"/> to the <see cref="LogEntryTable"/>.
                /// </summary>
                public const string ToLogEntry = "ImportToLogEntry";
            }

            /// <summary>
            /// Provides static values
            /// </summary>
            public static class Values
            {
                /// <summary>
                /// The id for "no attack".
                /// </summary>
                public const long AttackZeroId = 0;

                /// <summary>
                /// The text for "no attack"
                /// </summary>
                public const string AttackZeroText = "--";
            }
        }
        #endregion

        /************************************************************************/

        #region Constructor
        /// <summary>
        /// Initializes a new instance of the <see cref="AttackTable"/> class.
        /// </summary>
        public AttackTable() : base(Defs.TableName)
        {
        }
        #endregion

        /************************************************************************/

        #region Public methods
        /// <summary>
        /// Satisfies the abstract base class, but doesn't load anything.
        /// </summary>
        public override void Load()
        {
            Load("1=0", Defs.Columns.Id);
        }
        #endregion

        /************************************************************************/

        #region Protected methods
        protected override ColumnDefinitionCollection GetColumnDefinitions()
        {
            return new ColumnDefinitionCollection()
            {
                { Defs.Columns.Id, ColumnType.Integer, true },
                { Defs.Columns.AttackType, ColumnType.Integer },
                { Defs.Columns.AttackString, ColumnType.Text },
            };
        }

        /// <summary>
        /// Gets a list of column names to use in subsequent initial insert operations.
        /// These are used only when the table is empty, i.e. upon first creation.
        /// </summary>
        /// <returns>A list of column names</returns>
        protected override List<string> GetPopulateColumnList()
        {
            return new List<string>() { Defs.Columns.Id, Defs.Columns.AttackType, Defs.Columns.AttackString };
        }

        /// <summary>
        /// Provides an enumerable that returns values for each row to be populated.
        /// </summary>
        /// <returns>An IEnumerable</returns>
        protected override IEnumerable<object[]> EnumeratePopulateValues()
        {
            yield return new object[] { Defs.Values.AttackZeroId, AttackVectorType.None, Defs.Values.AttackZeroText };
        }
        #endregion

        /************************************************************************/

        #region Internal methods
        internal long Insert(LogEntry entry, AttackVectorType attackType)
        {
            foreach (AttackVector attack in entry.Attacks)
            {
                if (attack.AttackType == attackType)
                {
                    return Insert(attack);
                }
            }
            return Defs.Values.AttackZeroId;
        }

        private long Insert(AttackVector attack)
        {
            StringBuilder sql = new StringBuilder($"insert into {Namespace}.{TableName} (", 512);
            sql.Append($"{Defs.Columns.AttackType},");
            sql.Append($"{Defs.Columns.AttackString}) ");
            sql.Append("values (");

            sql.Append($"{attack.AttackTypeLong},");
            sql.Append($"'{attack.EscapedAttack}')");

            Controller.Execution.NonQuery(sql.ToString());

            object result = Controller.Execution.Scalar("select last_insert_rowid()");
            return result is long ? (long)result : Defs.Values.AttackZeroId;
        }
        #endregion
    }
}
