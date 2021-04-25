using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

using JonathanXmiq.Tools.Data.Formats.Options;

namespace JonathanXmiq.Tools.DataFormats.Csv
{
    /// <summary>
    /// A handler for csv files
    /// </summary>
    [Obsolete("Please Use JonathanXmiq.Tools.Data.Formats.CSV instead")]
    public class CSV : JonathanXmiq.Tools.Data.File
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

        /// <summary>
        /// Loads the csv file from disk.
        /// </summary>
        /// <param name="path">Csv file path.</param>
        public void Load(string path)
        {
            string[] file = File.ReadAllLines(path);
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
                    .Select(x => new CsvRow(this, x))
                    .ToArray();
            }
        }

        public override void AddRow(params string[][] RowData)
        {
            data = data?.Concat(RowData.Select(x => new CsvRow(this, x)))?.ToArray() ?? RowData.Select(x => new CsvRow(this, x)).ToArray();
        }

        /// <summary>
        /// Retrieves or sets the row at a specified column.
        /// </summary>
        /// <value>The row at the column.</value>
        public new IEnumerable<CsvCell> this[string Header]
        {
            get
            {
                return data.OfType<CsvRow>().Select(x => x[Header]);
            }
            set
            {
                int index = GetHeaderIndex(Header);
                for (int i = 0; i < value.Count(); i++)
                {
                    CsvCell cell = value.ElementAt(i);
                    if (cell.Header != Header)
                    {
                        throw new InvalidProgramException("Cell header does not match row header.");
                    }
                    data[i][index] = cell.RawData;
                }
            }
        }

        /// <summary>
        /// Retrieves or sets the row at a specified column index.
        /// </summary>
        /// <value>The row at the column index.</value>
        public new IEnumerable<CsvCell> this[int ColumnIndex]
        {
            get
            {
                return data.OfType<CsvRow>().Select(x => x[ColumnIndex]);
            }
            set
            {
                for (int i = 0; i < value.Count(); i++)
                {
                    CsvCell cell = value.ElementAt(i);
                    if (data[i] is CsvRow row)
                    {
                        if (cell.Header != row[ColumnIndex].Header)
                        {
                            throw new InvalidProgramException("Cell header does not match row header.");
                        }
                        row[ColumnIndex] = cell;
                    }
                }
            }
        }

        /// <summary>
        /// Retrieves or sets the Cell at a specified column and row number.
        /// </summary>
        /// <value>The Cell at the column and row number.</value>
        public new CsvCell this[string Header, int RowNumber]
        {
            get
            {
                return (data[RowNumber] as CsvRow)?[Header];
            }
            set
            {
                if (Header != value.Header)
                {
                    throw new InvalidProgramException("Cell header does not match row header.");
                }
                if (data[RowNumber] is CsvRow row)
                    row[Header] = value;
            }
        }

        /// <summary>
        /// Retrieves or sets the Cell at a specified column index and row number.
        /// </summary>
        /// <value>The Cell at the column index and row number.</value>
        public new CsvCell this[int ColumnIndex, int RowNumber]
        {
            get
            {
                return (data[RowNumber] as CsvRow)?[ColumnIndex];
            }
            set
            {
                if (data[RowNumber] is CsvRow row)
                {
                    if (value.Header != row[ColumnIndex].Header)
                    {
                        throw new InvalidProgramException("Cell header does not match row header.");
                    }
                    row[ColumnIndex] = value;
                }
            }
        }
    }
}