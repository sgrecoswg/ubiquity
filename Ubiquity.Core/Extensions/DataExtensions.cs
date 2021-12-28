using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace Ubiquity.Core.Extensions
{
    public static class DataExtensions
    {
        public static void Add(this List<object> Args, string Name, object Value)
        {
            Args.Add(new
            {
                ArgName = Name,
                ArgValue = Value,
            });
        }

        public static int? GetNullableInt32(this DataRow rowInfo, string ColumnName)
        {
            try
            {
                if (rowInfo.IsNull(ColumnName)) return new int?();
                return Convert.ToInt32(rowInfo[ColumnName]);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static decimal? GetNullableDecimal(this DataRow rowInfo, string ColumnName)
        {
            try
            {
                if (rowInfo.IsNull(ColumnName)) return new decimal?();
                return Convert.ToDecimal(rowInfo[ColumnName]);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static double? GetNullableDouble(this DataRow rowInfo, string ColumnName)
        {
            try
            {
                if (rowInfo.IsNull(ColumnName)) return new Double?();
                return Convert.ToDouble(rowInfo[ColumnName]);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static DateTime? GetNullableDateTime(this DataRow rowInfo, string ColumnName)
        {
            try
            {
                if (rowInfo.IsNull(ColumnName)) return new DateTime?();
                return Convert.ToDateTime(rowInfo[ColumnName]);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static bool? GetNullableBoolean(this DataRow rowInfo, string ColumnName)
        {
            try
            {
                if (rowInfo.IsNull(ColumnName)) return new bool?();
                return Convert.ToBoolean(rowInfo[ColumnName]);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static bool GetBoolean(this DataRow rowInfo, string ColumnName)
        {
            try
            {
                return Convert.ToBoolean(rowInfo[ColumnName]);
            }
            catch { throw; }
        }

        public static Guid GetGUID(this DataRow rowInfo, string ColumnName)
        {
            try
            {
                if (rowInfo.IsNull(ColumnName)) return Guid.Empty;
                return Guid.Parse(rowInfo.GetString(ColumnName));
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public static int GetInt32(this DataRow rowInfo, string ColumnName)
        {
            try
            {
                return Convert.ToInt32(rowInfo[ColumnName]);
            }
            catch { throw; }
        }

        public static string GetString(this DataRow rowInfo, string ColumnName)
        {
            try
            {
                return rowInfo[ColumnName].ToString();
            }
            catch { throw; }
        }

        public static decimal GetDecimal(this DataRow rowInfo, string ColumnName)
        {
            try
            {
                return Convert.ToDecimal(rowInfo[ColumnName]);
            }
            catch { throw; }
        }

        public static DateTime GetDateTime(this DataRow rowInfo, string ColumnName)
        {
            try
            {
                return Convert.ToDateTime(rowInfo[ColumnName]);
            }
            catch { throw; }
        }


        /// <summary>
        /// Converts a data table to a csv string
        /// </summary>
        /// <param name="dt">The data table we want to convert</param>
        /// <param name="delimeter">The delimeter we want to split the data with</param>
        /// <returns>string</returns>
        public static string AsCSV(this DataTable dt, string delimeter = ",")
        {
            StringBuilder _sb = new StringBuilder();
            IEnumerable<string> _columnNames = dt.Columns.Cast<System.Data.DataColumn>().Select(c => c.ColumnName);
            _sb.AppendLine(string.Join(delimeter, _columnNames));
            foreach (System.Data.DataRow row in dt.Rows)
            {
                IEnumerable<string> _fields = row.ItemArray.Select(f => f.ToString());
                _sb.AppendLine(string.Join(delimeter, _fields));
            }
            return _sb.ToString();
        }

        /// <summary>
        /// Checks to see if this row is in a list of rows
        /// </summary>
        /// <param name="dr">The data row we are checking</param>
        /// <param name="_rows">The list of rows we are looking in</param>
        /// <returns></returns>
        public static bool IsIn(this DataRow dr, List<DataRow> _rows)
        {
            foreach (DataRow r in _rows)
            {
                if (r.ItemArray == dr.ItemArray)
                {
                    return true;
                }
            }

            return false;
        }
    }
}
