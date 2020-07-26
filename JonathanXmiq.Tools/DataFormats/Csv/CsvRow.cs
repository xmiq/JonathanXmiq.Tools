using System.Linq;
using System.Collections;
using System.Collections.Generic;

namespace JonathanXmiq.Tools.DataFormats.Csv
{
    public class CsvRow : IEnumerable<CsvCell>
    {
        /// <summary>
        /// Contstructor for the class.
        /// </summary>
        /// <param name="cells">The cells in the row.</param>

        public CsvRow(CsvCell[] cells)
        {
            _cells = cells;
        }

        /// <summary>
        /// The Internal Data,
        /// </summary>
        /// <value>Data</value>
        private CsvCell[] _cells;

        /// <summary>
        /// Gets the enumerator for cells in the row.
        /// </summary>
        /// <returns>The Enumerator.</returns>
        public IEnumerator<CsvCell> GetEnumerator()
        {
            return _cells.AsEnumerable().GetEnumerator();
        }

        /// <summary>
        /// Gets the enumerator for cells in the row.
        /// </summary>
        /// <returns>The Enumerator.</returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return _cells.GetEnumerator();
        }

        /// <summary>
        /// Retrieves or sets the Cell at a specified column.
        /// </summary>
        /// <value>The Cell at the column.</value>
        public CsvCell this[string Header]
        {
            get
            {
                return _cells.FirstOrDefault(x => x.Header == Header);
            }
            set
            {
                _cells[_cells.Select((x, y) => new { x.Header, Index = y })
                     .FirstOrDefault(x => x.Header == Header)
                     .Index] = value;
            }
        }

        /// <summary>
        /// Retrieves or sets the Cell at a specified column index.
        /// </summary>
        /// <value>The Cell at the column index.</value>
        public CsvCell this[int index]
        {
            get
            {
                return _cells[index];
            }
            set
            {
                _cells[index] = value;
            }
        }
    }
}