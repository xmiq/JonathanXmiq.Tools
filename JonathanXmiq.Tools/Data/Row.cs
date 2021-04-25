using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JonathanXmiq.Tools.Data
{
    /// <summary>
    /// Data grid row container
    /// </summary>
    public class Row : IEnumerable<CellReference>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Row"/> class.
        /// </summary>
        /// <param name="data">The data.</param>
        internal Row(File parent, string[] data)
        {
            Parent = parent;
            this.data = data;
        }

        /// <summary>
        /// Gets or sets the parent file.
        /// </summary>
        /// <value>The parent file.</value>
        public File Parent { get; protected set; }

        /// <summary>
        /// Internal data
        /// </summary>
        protected string[] data;

        #region Header Methods

        /// <summary>
        /// Adds the column to the row.
        /// </summary>
        /// <param name="index">  The column index.</param>
        /// <param name="columns">The columns to add.</param>
        internal virtual void AddColumn(int index, string[] columns)
        {
            data = data.Take(index).Concat(columns).Concat(data.Skip(index)).ToArray();
        }

        /// <summary>
        /// Removes the column from the row.
        /// </summary>
        /// <param name="index">The column index.</param>
        internal virtual void RemoveColumn(int index)
        {
            data = data.Take(index).Concat(data.Skip(index + 1)).ToArray();
        }

        #endregion Header Methods

        #region IEnumearble Methods

        /// <summary>
        /// Gets the enumerator for cells in the row.
        /// </summary>
        /// <returns>The Enumerator.</returns>
        public IEnumerator<CellReference> GetEnumerator()
        {
            return Enumerable.Range(0, data.Length).Select(index => new CellReference(this, index)).GetEnumerator();
        }

        /// <summary>
        /// Gets the enumerator for cells in the row.
        /// </summary>
        /// <returns>The Enumerator.</returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        #endregion IEnumearble Methods

        #region Accessor Methods

        /// <summary>
        /// Retrieves or sets the Cell at a specified column.
        /// </summary>
        /// <value>The Cell at the column.</value>
        public string this[string Header]
        {
            get
            {
                return data[Parent.GetHeaderIndex(Header)];
            }
            set
            {
                data[Parent.GetHeaderIndex(Header)] = value;
            }
        }

        /// <summary>
        /// Retrieves or sets the Cell at a specified column index.
        /// </summary>
        /// <value>The Cell at the column index.</value>
        public string this[int index]
        {
            get
            {
                return data[index];
            }
            set
            {
                data[index] = value;
            }
        }

        #endregion Accessor Methods
    }
}