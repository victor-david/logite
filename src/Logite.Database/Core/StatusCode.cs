using Restless.Logite.Database.Tables;
using System.Collections.Generic;

namespace Restless.Logite.Database.Core
{
    /// <summary>
    /// Provides status code information.
    /// </summary>
    /// <remarks>
    /// This class contains definitions for supported status codes.
    /// Each code has a value (its http response such as 200, 404, etc)
    /// and a bit-mapped value that is used to specify which status codes
    /// a domain wants to display, stored in <see cref="DomainTable.Defs.Columns.ChartStatus"/>
    /// </remarks>
    public static class StatusCode
    {
        /// <summary>
        /// A simple struct to hold status value and its bit value.
        /// </summary>
        public struct StatusCodeItem
        {
            public long Value;
            public long BitValue;
            internal StatusCodeItem(long value, long bitValue)
            {
                Value = value;
                BitValue = bitValue;
            }
        }

        public static class Code200
        {
            public const long Value = 200;
            public const long BitValue = 1;
        }

        public static class Code302
        {
            public const long Value = 302;
            public const long BitValue = 32;
        }

        public static class Code304
        {
            public const long Value = 304;
            public const long BitValue = 64;
        }

        public static class Code400
        {
            public const long Value = 400;
            public const long BitValue = 512;
        }

        public static class Code404
        {
            public const long Value = 404;
            public const long BitValue = 1024;
        }

        public static class Code444
        {
            public const long Value = 444;
            public const long BitValue = 2048;
        }

        public static class Code500
        {
            public const long Value = 500;
            public const long BitValue = 16384;
        }

        /// <summary>
        /// Provides an enumerator that returns all <see cref="StatusCodeItem"/> objects.
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<StatusCodeItem> EnumerateAll()
        {
            yield return new StatusCodeItem(Code200.Value, Code200.BitValue);
            yield return new StatusCodeItem(Code302.Value, Code302.BitValue);
            yield return new StatusCodeItem(Code304.Value, Code304.BitValue);
            yield return new StatusCodeItem(Code400.Value, Code400.BitValue);
            yield return new StatusCodeItem(Code404.Value, Code404.BitValue);
            yield return new StatusCodeItem(Code444.Value, Code444.BitValue);
            yield return new StatusCodeItem(Code500.Value, Code500.BitValue);
        }
    }
}