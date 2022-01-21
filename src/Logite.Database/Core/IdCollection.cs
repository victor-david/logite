using System;
using System.Collections.Generic;
using System.Text;

namespace Restless.Logite.Database.Core
{
    /// <summary>
    /// Represents a collection of long id values.
    /// </summary>
    internal class IdCollection
    {
        private List<long> storage;

        public IdCollection()
        {
            storage = new List<long>();
        }

        public void Clear()
        {
            storage.Clear();
        }

        public void Add(long id)
        {
            if (!storage.Contains(id))
            {
                storage.Add(id);
            }
        }

        public override string ToString()
        {
            StringBuilder builder = new StringBuilder("-1");
            foreach (long id in storage)
            {
                builder.Append($",{id}");
            }
            return builder.ToString();
        }
    }
}