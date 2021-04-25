using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace JonathanXmiq.Tools.Data.Formats.Options
{
    /// <summary>
    /// Contains the options needed to parse a file
    /// </summary>
    public class FileOptions
    {
        /// <summary>
        /// Whether the Csv has headers.
        /// </summary>
        /// <value>If the Csv has headers.</value>
        public bool HasHeaders { get; set; } = true;

        /// <summary>
        /// The culture the dates for parsing dates.
        /// </summary>
        /// <value>DateTime culture.</value>
        public CultureInfo DateCulture { get; set; } = null;

        /// <summary>
        /// If the reader uses decimals or doubles/floats to store floating number values.
        /// </summary>
        /// <value>Use decimals to format data.</value>
        public bool UseDecimals { get; set; } = true;
    }
}