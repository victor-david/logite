namespace Restless.Logite.Database.Tables
{
    /// <summary>
    /// Represents a single row from a <see cref="RawTable{T}"/>
    /// </summary>
    public abstract class RawRow
    {
        /// <summary>
        /// The table name.
        /// </summary>
        public string TableName
        {
            get;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RawRow"/> class
        /// </summary>
        /// <param name="tableName"></param>
        protected RawRow(string tableName)
        {
            TableName = tableName;
        }
    }
}