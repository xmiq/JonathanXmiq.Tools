namespace JonathanXmiq.Tools.Data.Formats.Options
{
    /// <summary>
    /// Options Container for Handling CSV Files
    /// </summary>
    public class CsvFileOptions : FileOptions
    {
        /// <summary>
        /// The splitting / joining options character for a csv.
        /// </summary>
        /// <value>The splitting / joining options character.</value>
        public char SplitOptions { get; set; } = ',';
    }
}