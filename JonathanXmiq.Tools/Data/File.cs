using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;

using JonathanXmiq.Tools.Data.Formats.Options;

namespace JonathanXmiq.Tools.Data
{
    /// <summary>
    /// A generic handler for data grid files
    /// </summary>
    public class File : IEnumerable<Row>
    {
        /// <summary>
        /// Options for reading and writing the file.
        /// </summary>
        /// <value>Options for File.</value>
        public FileOptions Options { get; set; }

        /// <summary>
        /// All the column headers of the file.
        /// </summary>
        /// <value>The column headers.</value>
        public string[] Headers { get; protected set; }

        /// <summary>
        /// The data
        /// </summary>
        protected Row[] data;

        #region Header Methods

        /// <summary>
        /// Adds the headers to the file.
        /// </summary>
        /// <param name="Name">The header names.</param>
        public void AddHeader(params string[] Name) => AddHeader(0, Name);

        /// <summary>
        /// Adds the headers to the file.
        /// </summary>
        /// <param name="Name"> The header name.</param>
        /// <param name="index">The index to insert the header in.</param>
        public void AddHeader(string Name, int index) => AddHeader(index, Name);

        /// <summary>
        /// Adds the headers to the file.
        /// </summary>
        /// <param name="index">The index to insert the header in.</param>
        /// <param name="Name"> The header names.</param>
        public void AddHeader(int index, params string[] Name)
        {
            Headers = Headers.Take(index).Concat(Name).Concat(Headers.Skip(index)).ToArray();
            string[] cols = Enumerable.Repeat(string.Empty, Name.Length).ToArray();
            Array.ForEach(data, x => x.AddColumn(index, cols));
        }

        /// <summary>
        /// Removes the header at the specified index.
        /// </summary>
        /// <param name="Header">The header index.</param>
        public void RemoveHeader(int Header)
        {
            Headers = Headers.Take(Header).Concat(Headers.Skip(Header + 1)).ToArray();
            Array.ForEach(data, x => x.RemoveColumn(Header));
        }

        /// <summary>
        /// Removes the specified header.
        /// </summary>
        /// <param name="Header">The header name.</param>
        public void RemoveHeader(string Header) => RemoveHeader(GetHeaderIndex(Header));

        #endregion Header Methods

        #region Add Row

        /// <summary>
        /// Adds new rows.
        /// </summary>
        /// <param name="RowData">The row data.</param>
        public virtual void AddRow(params string[][] RowData)
        {
            data = data?.Concat(RowData.Select(x => new Row(this, x)))?.ToArray() ?? RowData.Select(x => new Row(this, x)).ToArray();
        }

        #endregion Add Row

        #region IEnumearble Methods

        /// <summary>
        /// Gets the enumerator for rows in the file.
        /// </summary>
        /// <returns>The Enumerator.</returns>
        public IEnumerator<Row> GetEnumerator()
        {
            return data.AsEnumerable().GetEnumerator();
        }

        /// <summary>
        /// Gets the enumerator for rows in the file.
        /// </summary>
        /// <returns>The Enumerator.</returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return data.GetEnumerator();
        }

        #endregion IEnumearble Methods

        /// <summary>
        /// Gets the index of the header.
        /// </summary>
        /// <param name="Header">The header name.</param>
        /// <returns>The header index.</returns>
        /// <exception cref="InvalidProgramException">Cell header does not match row header.</exception>
        internal int GetHeaderIndex(string Header)
        {
            int index = Array.IndexOf(Headers, Header);
            if (index == -1)
                throw new InvalidProgramException("Cell header does not match row header.");
            return index;
        }

        /// <summary>
        /// Retrieves or sets the row at a specified column.
        /// </summary>
        /// <value>The row at the column.</value>
        public IEnumerable<string> this[string Header]
        {
            get
            {
                return data.Select(x => x[Header]);
            }
            set
            {
                int index = GetHeaderIndex(Header);
                for (int i = 0; i < value.Count(); i++)
                {
                    data[i][index] = value.ElementAt(i);
                }
            }
        }

        /// <summary>
        /// Retrieves or sets the row at a specified column index.
        /// </summary>
        /// <value>The row at the column index.</value>
        public IEnumerable<string> this[int ColumnIndex]
        {
            get
            {
                return data.Select(x => x[ColumnIndex]);
            }
            set
            {
                for (int i = 0; i < value.Count(); i++)
                {
                    data[i][ColumnIndex] = value.ElementAt(i);
                }
            }
        }

        /// <summary>
        /// Retrieves the cell reference at a specified column and row number.
        /// </summary>
        /// <value>The Cell reference at the column and row number.</value>
        public CellReference this[string Header, int RowNumber] => new CellReference(data[RowNumber], GetHeaderIndex(Header));

        /// <summary>
        /// Retrieves the cell reference at a specified column index and row number.
        /// </summary>
        /// <value>The Cell Reference at the column index and row number.</value>
        public CellReference this[int ColumnIndex, int RowNumber] => new CellReference(data[RowNumber], ColumnIndex);

        public T Convert<T>()
            where T : File, new()
        {
            return new T
            {
                Options = Options,
                Headers = Headers,
                data = data
            };
        }
    }
}