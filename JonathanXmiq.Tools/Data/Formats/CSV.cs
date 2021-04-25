using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

using JonathanXmiq.Tools.Data.Formats.Options;

namespace JonathanXmiq.Tools.Data.Formats
{
    /// <summary>
    /// A handler for csv files
    /// </summary>
    public class CSV : File
    {
        /// <summary>
        /// Gets or sets the CSV options.
        /// </summary>
        /// <value>The CSV options.</value>
        public CsvFileOptions CsvOptions
        {
            get => Options as CsvFileOptions;
            set => Options = value;
        }

        #region File Read

        /// <summary>
        /// Loads the csv file from disk.
        /// </summary>
        /// <param name="path">Csv file path.</param>
        public void Load(string path)
        {
            string[] file = System.IO.File.ReadAllLines(path);
            ReadCsv(file);
        }

        /// <summary>
        /// Loads the csv file from steam.
        /// </summary>
        /// <param name="fileStream">Csv file.</param>
        public void Load(Stream fileStream)
        {
            StreamReader sr = new StreamReader(fileStream);
            List<string> file = new List<string>();
            while (fileStream.Position < fileStream.Length)
            {
                file.Add(sr.ReadLine());
            }
            ReadCsv(file.ToArray());
        }

        /// <summary>
        /// Loads the csv file from steam asyncronously.
        /// </summary>
        /// <param name="fileStream">Csv file.</param>
        public async Task LoadAsync(Stream fileStream)
        {
            StreamReader sr = new StreamReader(fileStream);
            List<string> file = new List<string>();
            while (fileStream.Position < fileStream.Length)
            {
                file.Add(await sr.ReadLineAsync().ConfigureAwait(false));
            }
            ReadCsv(file.ToArray());
        }

        #endregion File Read

        /// <summary>
        /// Parses the csv data from a string array.
        /// </summary>
        /// <param name="csv">Csv data.</param>
        public void ReadCsv(string[] csv)
        {
            IEnumerable<string[]> rows = csv.Select(x => x.Split(CsvOptions?.SplitOptions ?? ','));

            if (Options.HasHeaders)
            {
                Headers = rows.FirstOrDefault()
                    ?.Select(x => x.Trim())
                    ?.ToArray();

                data = rows.Skip(1)
                    .Select(x => new Row(this, x))
                    .ToArray();
            }
        }
    }
}