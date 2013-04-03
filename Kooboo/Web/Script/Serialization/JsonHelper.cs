using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web.Compilation;
using System.Web.Configuration;
using System.Web.Script.Serialization;

namespace Kooboo.Web.Script.Serialization
{
    public static class JsonHelper
    {
        static readonly long DatetimeMinTimeTicks = new DateTime(0x7b2, 1, 1, 0, 0, 0, DateTimeKind.Utc).Ticks;

        #region static
        static JavaScriptSerializer jsonSerializer = new JavaScriptSerializer();
        static JsonHelper()
        {
            ScriptingJsonSerializationSection settings = (ScriptingJsonSerializationSection)ConfigurationManager.GetSection("system.web.extensions/scripting/webServices/jsonSerialization");
            jsonSerializer.MaxJsonLength = settings.MaxJsonLength;
            jsonSerializer.RecursionLimit = settings.RecursionLimit;
            jsonSerializer.RegisterConverters(CreateConverters(settings.Converters));
        }
        internal static JavaScriptConverter[] CreateConverters(ConvertersCollection converters)
        {
            List<JavaScriptConverter> list = new List<JavaScriptConverter>();
            foreach (Converter converter in converters)
            {
                Type c = BuildManager.GetType(converter.Type, false);
                list.Add((JavaScriptConverter)Activator.CreateInstance(c));
            }
            return list.ToArray();
        }

        /// <summary>
        /// Registers the converter.
        /// </summary>
        /// <param name="converter">The converter.</param>
        public static void RegisterConverter(JavaScriptConverter converter)
        {
            jsonSerializer.RegisterConverters(new[] { converter });
        }
        #endregion

        /// <summary>
        /// To the JSON.
        /// </summary>
        /// <param name="obj">The obj.</param>
        /// <returns></returns>
        public static string ToJSON(this object obj)
        {
            return jsonSerializer.Serialize(obj);
        }

        /// <summary>
        /// Deserializes from JSON.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="json">The json.</param>
        /// <returns></returns>
        public static T Deserialize<T>(string json)
        {
            return jsonSerializer.Deserialize<T>(json);
        }

        /// <summary>
        /// Jsons the string to date time.
        /// </summary>
        /// <param name="s">The s.</param>
        /// <returns></returns>
        public static DateTime JsonStringToDateTime(string s)
        {
            long num;
            Match match = Regex.Match(s, "^/Date\\((?<ticks>-?[0-9]+)(?:[a-zA-Z]|(?:\\+|-)[0-9]{4})?\\)/");
            if (long.TryParse(match.Groups["ticks"].Value, out num))
            {
                return new DateTime((num * 0x2710L) + DatetimeMinTimeTicks, DateTimeKind.Utc);
            }
            return DateTime.MinValue;
        }

        #region QuoteString
        /// <summary>
        /// Quotes the string.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public static string QuoteString(string value)
        {
            StringBuilder builder = null;
            if (string.IsNullOrEmpty(value))
            {
                return string.Empty;
            }
            int startIndex = 0;
            int count = 0;
            for (int i = 0; i < value.Length; i++)
            {
                char c = value[i];
                if ((((c == '\r') || (c == '\t')) || ((c == '"') || (c == '\''))) || ((((c == '<') || (c == '>')) || ((c == '\\') || (c == '\n'))) || (((c == '\b') || (c == '\f')) || (c < ' '))))
                {
                    if (builder == null)
                    {
                        builder = new StringBuilder(value.Length + 5);
                    }
                    if (count > 0)
                    {
                        builder.Append(value, startIndex, count);
                    }
                    startIndex = i + 1;
                    count = 0;
                }
                switch (c)
                {
                    case '<':
                    case '>':
                    case '\'':
                        {
                            AppendCharAsUnicode(builder, c);
                            continue;
                        }
                    case '\\':
                        {
                            builder.Append(@"\\");
                            continue;
                        }
                    case '\b':
                        {
                            builder.Append(@"\b");
                            continue;
                        }
                    case '\t':
                        {
                            builder.Append(@"\t");
                            continue;
                        }
                    case '\n':
                        {
                            builder.Append(@"\n");
                            continue;
                        }
                    case '\f':
                        {
                            builder.Append(@"\f");
                            continue;
                        }
                    case '\r':
                        {
                            builder.Append(@"\r");
                            continue;
                        }
                    case '"':
                        {
                            builder.Append("\\\"");
                            continue;
                        }
                }
                if (c < ' ')
                {
                    AppendCharAsUnicode(builder, c);
                }
                else
                {
                    count++;
                }
            }
            if (builder == null)
            {
                return value;
            }
            if (count > 0)
            {
                builder.Append(value, startIndex, count);
            }
            return builder.ToString();
        }

        /// <summary>
        /// Appends the char as unicode.
        /// </summary>
        /// <param name="builder">The builder.</param>
        /// <param name="c">The c.</param>
        private static void AppendCharAsUnicode(StringBuilder builder, char c)
        {
            builder.Append(@"\u");
            builder.AppendFormat(CultureInfo.InvariantCulture, "{0:x4}", new object[] { (int)c });
        }


        /// <summary>
        /// Quotes the string.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="addQuotes">if set to <c>true</c> [add quotes].</param>
        /// <returns></returns>
        public static string QuoteString(string value, bool addQuotes)
        {
            string str = QuoteString(value);
            if (addQuotes)
            {
                str = "\"" + str + "\"";
            }
            return str;
        }
        #endregion
    }
}
