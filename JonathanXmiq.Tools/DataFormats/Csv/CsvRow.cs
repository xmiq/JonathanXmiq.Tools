using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System;
using JonathanXmiq.Tools.Data;

namespace JonathanXmiq.Tools.DataFormats.Csv
{
    [Obsolete]
    public class CsvRow : Row
    {
        /// <summary>
        /// Contstructor for the class.
        /// </summary>
        /// <param name="cells">The cells in the row.</param>
        public CsvRow(CSV parent, string[] cells)
            : base(parent, cells)
        {
            _cells = cells.Select((y, z) => new CsvCell(this, z)
            {
                RawData = y
            })
            .ToArray();
        }

        /// <summary>
        /// Contstructor for the class.
        /// </summary>
        /// <param name="cells">The cells in the row.</param>
        /// <exception cref="NotSupportedException"></exception>
        public CsvRow(CsvCell[] cells)
            : base(null, cells.Select(x => x.RawData).ToArray())
        {
            throw new NotSupportedException();
        }

        /// <summary>
        /// The Internal Data,
        /// </summary>
        /// <value>Data</value>
        private CsvCell[] _cells;

        #region Header Methods

        /// <summary>
        /// Adds the column to the row.
        /// </summary>
        /// <param name="index">  The column index.</param>
        /// <param name="columns">The columns to add.</param>
        internal override void AddColumn(int index, string[] columns)
        {
            base.AddColumn(index, columns);
            _cells = _cells.Take(index).Concat(columns.Select((y, z) => new CsvCell(this, z)
            {
                RawData = y
            })).Concat(_cells.Skip(index)).ToArray();
        }

        /// <summary>
        /// Removes the column from the row.
        /// </summary>
        /// <param name="index">The column index.</param>
        internal override void RemoveColumn(int index)
        {
            base.RemoveColumn(index);
            _cells = _cells.Take(index).Concat(_cells.Skip(index + 1)).ToArray();
        }

        #endregion Header Methods

        /// <summary>
        /// Retrieves or sets the Cell at a specified column.
        /// </summary>
        /// <value>The Cell at the column.</value>
        public new CsvCell this[string Header]
        {
            get => Array.Find(_cells, x => x.Header == Header);
            set => _cells[Parent.GetHeaderIndex(Header)] = value;
        }

        /// <summary>
        /// Retrieves or sets the Cell at a specified column index.
        /// </summary>
        /// <value>The Cell at the column index.</value>
        public new CsvCell this[int index]
        {
            get => _cells[index];
            set => _cells[index] = value;
        }
    }
}