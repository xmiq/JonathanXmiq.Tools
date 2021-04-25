using System;
using System.Globalization;
using System.Security.Cryptography;

using JonathanXmiq.Tools.Data;

namespace JonathanXmiq.Tools.DataFormats.Csv
{
    /// <summary>
    /// The Cell container
    /// </summary>
    [Obsolete]
    public class CsvCell : CellReference
    {
        /// <summary>
        /// Contstructor for the class.
        /// </summary>
        /// <param name="header">The row header.</param>
        /// <exception cref="NotSupportedException"></exception>
        [Obsolete]
        public CsvCell(CsvHeader header)
            : base(null, -1)
        {
            throw new NotSupportedException();
        }

        public CsvCell(Row Parent, int Header)
            : base(Parent, Header)
        {
        }
    }
}