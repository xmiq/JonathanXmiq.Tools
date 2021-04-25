using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace JonathanXmiq.Tools.Data
{
    /// <summary>
    /// References a cell in a row
    /// </summary>
    public class CellReference
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CellReference"/> class.
        /// </summary>
        /// <param name="parent">The parent row.</param>
        /// <param name="index"> The Header index.</param>
        internal CellReference(Row parent, int index)
        {
            Parent = parent;
            HeaderIndex = index;
        }

        /// <summary>
        /// Gets or sets the parent row.
        /// </summary>
        /// <value>The parent row.</value>
        public Row Parent { get; internal set; }

        /// <summary>
        /// Gets or sets the header index.
        /// </summary>
        /// <value>The header index.</value>
        public int HeaderIndex { get; internal set; }

        /// <summary>
        /// Retrieves the Header Name.
        /// </summary>
        public string Header => Parent.Parent.Headers[HeaderIndex];

        /// <summary>
        /// The Csv Raw Data.
        /// </summary>
        /// <value>Raw Data.</value>
        public string RawData
        {
            get => Parent[HeaderIndex];
            set => Parent[HeaderIndex] = value;
        }

        /// <summary>
        /// Parsed Value.
        /// </summary>
        /// <value>Parsed Value.</value>
        public dynamic Value
        {
            get
            {
                if (Parent.Parent.Options?.DateCulture != null && DateTime.TryParse(RawData, Parent.Parent.Options.DateCulture, DateTimeStyles.None, out DateTime parsedCulture))
                {
                    return parsedCulture;
                }
                else if (DateTime.TryParse(RawData, out DateTime parsed))
                {
                    return parsed;
                }
                else if (byte.TryParse(RawData, out byte parsedByte))
                {
                    return parsedByte;
                }
                else if (short.TryParse(RawData, out short parsedShort))
                {
                    return parsedShort;
                }
                else if (int.TryParse(RawData, out int parsedInt))
                {
                    return parsedInt;
                }
                else if (long.TryParse(RawData, out long parsedLong))
                {
                    return parsedLong;
                }
                else if ((Parent.Parent.Options?.UseDecimals ?? true) && decimal.TryParse(RawData, out decimal parsedDecimal))
                {
                    return parsedDecimal;
                }
                else if (!(Parent.Parent.Options?.UseDecimals ?? true) && float.TryParse(RawData, out float parsedFloat))
                {
                    return parsedFloat;
                }
                else if (!(Parent.Parent.Options?.UseDecimals ?? true) && double.TryParse(RawData, out double parsedDouble))
                {
                    return parsedDouble;
                }
                else if (bool.TryParse(RawData, out bool parsedBool))
                {
                    return parsedBool;
                }
                else
                {
                    return RawData;
                }
            }
            set
            {
                RawData = (value as object)?.ToString();
            }
        }

        /// <summary>
        /// Retrieves the value in the specified type.
        /// </summary>
        /// <typeparam name="T">The type of the value.</typeparam>
        /// <returns>The value of the cell.</returns>
        public T GetValue<T>()
            where T : IConvertible
        {
            IConvertible toReturn = Type.GetTypeCode(typeof(T)) switch
            {
                TypeCode.DateTime => (Parent.Parent.Options?.DateCulture != null && DateTime.TryParse(RawData, Parent.Parent.Options.DateCulture, DateTimeStyles.None, out DateTime parsedCulture)) ? parsedCulture : (DateTime.TryParse(RawData, out DateTime parsed)) ? parsed : default(IConvertible),
                TypeCode.Byte => byte.TryParse(RawData, out byte parsedByte) ? parsedByte : default(IConvertible),
                TypeCode.Int16 => short.TryParse(RawData, out short parsedShort) ? parsedShort : default(IConvertible),
                TypeCode.Int32 => int.TryParse(RawData, out int parsedInt) ? parsedInt : default(IConvertible),
                TypeCode.Int64 => long.TryParse(RawData, out long parsedLong) ? parsedLong : default(IConvertible),
                TypeCode.Decimal => decimal.TryParse(RawData, out decimal parsedDecimal) ? parsedDecimal : default(IConvertible),
                TypeCode.Single => float.TryParse(RawData, out float parsedFloat) ? parsedFloat : default(IConvertible),
                TypeCode.Double => double.TryParse(RawData, out double parsedDouble) ? parsedDouble : default(IConvertible),
                TypeCode.Boolean => bool.TryParse(RawData, out bool parsedBool) ? parsedBool : default(IConvertible),
                _ => default
            };

            return (T)toReturn;
        }
    }
}