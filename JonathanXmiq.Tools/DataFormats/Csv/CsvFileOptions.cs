using System.Globalization;
namespace JonathanXmiq.Tools.DataFormats.Csv
{
    /// <summary>
    /// Options Container for Handling CSV Files
    /// </summary>
    public class CsvFileOptions
    {
        /// <summary>
        /// The splitting / joining options character for a csv.
        /// </summary>
        /// <value></value>
        public char SplitOptions { get; set; } = ',';

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