using Restless.Toolkit.Core.Database.SQLite;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace Restless.Logite.Database.Tables
{
    public abstract class RawRow
    {
        public string TableName
        {
            get;
        }

        protected RawRow(string tableName)
        {
            TableName = tableName;
        }

        protected string GetString(string colName)
        {
            return string.Empty;
        }

        protected long GetInt64(string column)
        {
            return 0;
        }

        protected DateTime GetDateTime(string column)
        {
            //if (Row[colName] != DBNull.Value)
            //{
            //    return (DateTime)Row[colName];
            //}
            //else
            {
                return DateTime.MinValue;
            }
        }

    }
}
