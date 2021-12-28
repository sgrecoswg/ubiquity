using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Text;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Ubiquity.Core.Extensions
{
    public static class XmlWriterExtension
    {
        public static void WriteAttributeBoolean(this XmlWriter xw, string localName, bool value)
        {
            xw.WriteAttributeString(localName, value ? "Y" : "N");
        }

        public static void WriteAttributeInt32(this XmlWriter xw, string localName, int value)
        {
            xw.WriteAttributeString(localName, value.ToString());
        }

        public static void WriteAttributeFloat(this XmlWriter xw, string localName, float value)
        {
            xw.WriteAttributeString(localName, value.ToString());
        }

        /// <summary>
        /// Writes a Nullable int32 attribute to XML.  DOES NOT WRITE ATTRIBUTE if the value is null, but writes a string if it's a value.
        /// </summary>
        /// <param name="xw">Xml Writer to write to.</param>
        /// <param name="localName">The local name of the attribute</param>
        /// <param name="value">The value of the attribute.</param>
        public static void WriteAttributeNullableInt32(this XmlWriter xw, string localName, int? value)
        {
            if (value.HasValue)
            {
                xw.WriteAttributeString(localName, value.Value.ToString());
            }
        }

        /// <summary>
        /// Writes a System.Drawing.Color out as ARGB hex.
        /// </summary>
        /// <param name="xw">Xml Writer to write to.</param>
        /// <param name="localName">The local name of the attribute</param>
        /// <param name="value">The value of the attribute.</param>
        public static void WriteAttributeColor(this XmlWriter xw, string localName, Color value)
        {
            if (value == null) return;
            string colorValue = string.Format("{0:X2}{1:X2}{2:X2}{3:X2}", value.A, value.R, value.G, value.B);
            xw.WriteAttributeString(localName, colorValue);
        }

        public static void WriteAttributeTextRenderingHint(this XmlWriter xw, string localName, TextRenderingHint value)
        {
            if (value == 0) return;
            string textValue = value.ToString();
            xw.WriteAttributeString(localName, textValue);
        }

        public static void WriteAttributeContentAlignment(this XmlWriter xw, string localName, ContentAlignment value)
        {
            if (value == 0) return;
            string textValue = value.ToString();
            xw.WriteAttributeString(localName, textValue);
        }
        /// <summary>
        /// Parses a string and returns the enumeration value, if valid.
        /// </summary>
        /// <typeparam name="T">The type of enumeration</typeparam>
        /// <param name="value">The string value of the enumeration.</param>
        /// <returns></returns>
        public static T ToEnum<T>(this string value)
        {
            return (T)Enum.Parse(typeof(T), value, true);
        }

    }

}
