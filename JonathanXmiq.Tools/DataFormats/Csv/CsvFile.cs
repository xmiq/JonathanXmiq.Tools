using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace JonathanXmiq.Tools.DataFormats.Csv
{
    /// <summary>
    /// A handler for csv files
    /// </summary>
     public class CsvFile : IEnumerable<CsvRow>
    {
        /// <summary>
        /// Options for reading and writing the csv file.
        /// </summary>
        /// <value>Options for CsvFile.</value>
        public CsvFileOptions Options { get; set; }

        /// <summary>
        /// Row data.
        /// </summary>
        private CsvRow[] internalData;

        /// <summary>
        /// Header data.
        /// </summary>
        private CsvHeader[] internalHeaders;

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
            while(fileStream.Position < fileStream.Length)
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
            while(fileStream.Position < fileStream.Length)
            {
                file.Add(await sr.ReadLineAsync());
            }
            ReadCsv(file.ToArray());
        }

        /// <summary>
        /// Parses the csv data from a string array.
        /// </summary>
        /// <param name="csv">Csv data.</param>
        public void ReadCsv(string[] csv)
        {
            IEnumerable<string[]> rows = csv.Select(x => x.Split(Options.SplitOptions));

            if (Options.HasHeaders)
            {
                internalHeaders = rows.FirstOrDefault()
                    .Select(x => new CsvHeader { Name = x.Trim() })
                    .ToArray();

                internalData = rows.Skip(1)
                    .Select(x => new CsvRow( x.Select((y,z ) => new CsvCell(internalHeaders[z]) 
                        {
                            Options = this.Options,
                            RawData = y
                        })
                        .ToArray()))
                    .ToArray();
            }
        }

        /// <summary>
        /// Gets the enumerator for rows in the file.
        /// </summary>
        /// <returns>The Enumerator.</returns>
        public IEnumerator<CsvRow> GetEnumerator()
        {
            return internalData.AsEnumerable().GetEnumerator();
        }

        /// <summary>
        /// Gets the enumerator for rows in the file.
        /// </summary>
        /// <returns>The Enumerator.</returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return internalData.GetEnumerator();
        }

        /// <summary>
        /// Retrieves or sets the row at a specified column.
        /// </summary>
        /// <value>The row at the column.</value>

        public IEnumerable<CsvCell> this[string Header]
        {
            get
            {
                return internalData.Select(x => x[Header]);
            }
            set
            {
                for(int i = 0; i < value.Count(); i++)
                {
                    CsvCell cell = value.ElementAt(i);
                    if (cell.Header != Header)
                    {
                        throw new InvalidProgramException("Cell header does not match row header.");
                    }
                    internalData[i][Header] = cell;
                }
            }
        }

        /// <summary>
        /// Retrieves or sets the row at a specified column index.
        /// </summary>
        /// <value>The row at the column index.</value>
        public IEnumerable<CsvCell> this[int ColumnIndex]
        {
            get
            {
                return internalData.Select(x => x[ColumnIndex]);
            }
            set
            {
                for(int i = 0; i < value.Count(); i++)
                {
                    CsvCell cell = value.ElementAt(i);
                    if (cell.Header != internalData[i][ColumnIndex].Header)
                    {
                        throw new InvalidProgramException("Cell header does not match row header.");
                    }
                    internalData[i][ColumnIndex] = cell;
                }
            }
        }

        /// <summary>
        /// Retrieves or sets the Cell at a specified column and row number.
        /// </summary>
        /// <value>The Cell at the column and row number.</value>
        public CsvCell this[string Header, int RowNumber]
        {
            get
            {
                return internalData[RowNumber][Header];
            }
            set
            {       
                if (Header != value.Header)
                {
                    throw new InvalidProgramException("Cell header does not match row header.");
                }
                internalData[RowNumber][Header] = value;
            }
        }

        /// <summary>
        /// Retrieves or sets the Cell at a specified column index and row number.
        /// </summary>
        /// <value>The Cell at the column index and row number.</value>
        public CsvCell this[int ColumnIndex, int RowNumber]
        {
            get
            {
                return internalData[RowNumber][ColumnIndex];
            }
            set
            {
                if (value.Header != internalData[RowNumber][ColumnIndex].Header)
                {
                    throw new InvalidProgramException("Cell header does not match row header.");
                }
                internalData[RowNumber][ColumnIndex] = value;
            }
        }
    }
}
