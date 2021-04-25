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
            fileStream.Position = 0;
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
                await sr.ReadLineAsync().ConfigureAwait(false);
            }
            ReadCsv(file.ToArray());
            fileStream.Position = 0;
        }

        #endregion File Read

        #region File Write

        /// <summary>
        /// Writes the csv file to disk.
        /// </summary>
        /// <param name="path">Csv file path.</param>
        public void Write(string path) => System.IO.File.WriteAllLines(path, WriteCsv());

        /// <summary>
        /// Writes the csv file to steam.
        /// </summary>
        /// <param name="fileStream">Csv file.</param>
        public void Write(Stream fileStream)
        {
            StreamWriter sw = new StreamWriter(fileStream);
            foreach (string s in WriteCsv())
            {
                sw.WriteLine(s);
            }
            fileStream.Position = 0;
        }

        /// <summary>
        /// Writes the csv file to steam asyncronously.
        /// </summary>
        /// <param name="fileStream">Csv file.</param>
        public async Task WriteAsync(Stream fileStream)
        {
            StreamWriter sw = new StreamWriter(fileStream);
            foreach (string s in WriteCsv())
            {
                await sw.WriteLineAsync(s);
            }
            fileStream.Position = 0;
        }

        #endregion File Write

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

        public IEnumerable<string> WriteCsv()
        {
            string separator = CsvOptions.SplitOptions.ToString();
            yield return string.Join(separator, Headers);
            foreach (var itm in data)
            {
                yield return string.Join(separator, itm);
            }
        }
    }