using System;
using System.Globalization;

namespace JonathanXmiq.Tools.DataFormats.Csv
{
    /// <summary>
    /// The Cell container
    /// </summary>
    public class CsvCell
    {
        /// <summary>
        /// Contstructor for the class.
        /// </summary>
        /// <param name="header">The row header.</param>
        public CsvCell(CsvHeader header)
        {
            _header = header;
        }

        /// <summary>
        /// The Header for the row.
        /// </summary>
        private CsvHeader _header;

        /// <summary>
        /// Retrieves the Header Name.
        /// </summary>
        public string Header => _header.Name;
        
        /// <summary>
        /// File Parsing Options.
        /// </summary>
        /// <value>File Parsing Options.</value>
        public CsvFileOptions Options { get; set;}

        /// <summary>
        /// The Csv Raw Data.
        /// </summary>
        /// <value>Raw Data.</value>
        public string RawData { get; set; }

        /// <summary>
        /// Parsed Value.
        /// </summary>
        /// <value>Parsed Value.</value>
        public dynamic Value 
        {
            get
            {
                if (Options?.DateCulture != null && DateTime.TryParse(RawData, Options.DateCulture, DateTimeStyles.None, out DateTime parsedCulture))
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
                else if ((Options?.UseDecimals ?? true) && decimal.TryParse(RawData, out decimal parsedDecimal))
                {
                    return parsedDecimal;
                }
                else if (!(Options?.UseDecimals ?? true) && float.TryParse(RawData, out float parsedFloat))
                {
                    return parsedFloat;
                }
                else if (!(Options?.UseDecimals ?? true) && double.TryParse(RawData, out double parsedDouble))
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
                RawData = (value as object).ToString();
            }
        }

        /// <summary>
        /// Retrieves the value in the specified type.
        /// </summary>
        /// <typeparam name="T">The type of the value.</typeparam>
        /// <returns>The value of the cell.</returns>
        public T GetValue<T>()
            where T: IConvertible
        {
            IConvertible toReturn = Type.GetTypeCode(typeof(T)) switch 
            {
                TypeCode.DateTime => (Options?.DateCulture != null && DateTime.TryParse(RawData, Options.DateCulture, DateTimeStyles.None, out DateTime parsedCulture)) ? parsedCulture : (DateTime.TryParse(RawData, out DateTime parsed)) ?  parsed : default(IConvertible),
                TypeCode.Byte => byte.TryParse(RawData, out byte parsedByte) ? parsedByte : default(IConvertible),
                TypeCode.Int16 => short.TryParse(RawData, out short parsedShort) ? parsedShort : default(IConvertible),
                TypeCode.Int32 => int.TryParse(RawData, out int parsedInt) ? parsedInt : default(IConvertible),
                TypeCode.Int64 => long.TryParse(RawData, out long parsedLong) ? parsedLong : default(IConvertible),
                TypeCode.Decimal => decimal.TryParse(RawData, out decimal parsedDecimal) ? parsedDecimal : default(IConvertible),
                TypeCode.Single => float.TryParse(RawData, out float parsedFloat) ? parsedFloat : default(IConvertible),
                TypeCode.Double => double.TryParse(RawData, out double parsedDouble) ? parsedDouble : default(IConvertible),
                TypeCode.Boolean => bool.TryParse(RawData, out bool parsedBool) ? parsedBool : default(IConvertible),
                _ => default(IConvertible)
            };
            
            return (T) toReturn;
        }
    }
}