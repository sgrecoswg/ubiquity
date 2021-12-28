using System;
using System.Linq;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

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

       

    }

    public static class AdoNetExtensions
    {
        public static bool HasResults(this DataSet xTest)
        {
            if (xTest == null) return false;
            if (xTest.Tables.Count == 0) return false;
            if (xTest.Tables[0].Rows.Count == 0) return false;
            return true;
        }

        public static DataRow GetFirstDataRowInDataSet(this DataSet dsInfo)
        {
            try
            {
                return dsInfo.Tables[0].Rows[0];
            }
            catch
            {
                return null;
            }
        }


        public static DataSet OnlyTablesByNames(this DataSet ds, params string[] names)
        {
            var newDS = new DataSet() { DataSetName = ds.DataSetName };

            foreach (DataTable dt in ds.Tables)
            {
                if (names.Contains(dt.TableName))
                {
                    var ndt = new DataTable() { TableName = dt.TableName };
                    ndt.Merge(dt);
                    newDS.Tables.Add(ndt);
                }
            }

            return newDS;
        }

        public static DataSet Where(this DataSet ds, Func<DataTable, bool> fn)
        {
            var newDS = new DataSet() { DataSetName = ds.DataSetName };

            foreach (DataTable dt in ds.Tables)
            {
                if (fn(dt))
                {
                    var ndt = new DataTable() { TableName = dt.TableName };
                    ndt.Merge(dt);
                    newDS.Tables.Add(ndt);
                }
            }

            return newDS;
        }

        public static DataTableCollection Where(this DataTableCollection coll, Func<DataTable, bool> fn)
        {
            var newDS = new DataSet();

            foreach (DataTable dt in coll)
            {
                if (fn(dt))
                {
                    var ndt = new DataTable() { TableName = dt.TableName };
                    ndt.Merge(dt);
                    newDS.Tables.Add(ndt);
                }
            }

            return newDS.Tables;
        }



        public static int TotalRows(this DataSet ds)
        {
            int records = 0;

            foreach (DataTable dt in ds.Tables)
            {
                records += dt.Rows.Count;
            }

            return records;
        }

        public static int TotalRows(this DataTableCollection coll)
        {
            int records = 0;

            foreach (DataTable dt in coll)
            {
                records += dt.Rows.Count;
            }

            return records;
        }

        public static string InferString(this DataRow dr, string columnName)
        {
            if (!dr.Table.Columns.Contains(columnName) || dr.IsNull(columnName))
            {
                return string.Empty;
            }

            return dr[columnName].ToString();
        }

        public static int InferInt(this DataRow dr, string columnName)
        {

            if (!dr.Table.Columns.Contains(columnName) || dr.IsNull(columnName))
            {
                return 0;
            }

            return Convert.ToInt32(dr[columnName]);
        }

        public static decimal InferDecimal(this DataRow dr, string columnName)
        {
            if (!dr.Table.Columns.Contains(columnName) || dr.IsNull(columnName))
            {
                return 0;
            }

            return Convert.ToDecimal(dr[columnName]);
        }

        public static bool InferBool(this DataRow dr, string columnName)
        {
            if (!dr.Table.Columns.Contains(columnName) || dr.IsNull(columnName))
            {
                return false;
            }

            return Convert.ToBoolean(dr[columnName]);
        }

        public static decimal? InferDecimal(this DataRow dr, string columnName, decimal? defaultValue)
        {
            if (!dr.Table.Columns.Contains(columnName) || dr.IsNull(columnName))
            {
                return defaultValue;
            }

            return Convert.ToDecimal(dr[columnName]);
        }

        public static int? InferInt(this DataRow dr, string columnName, int? defaultValue)
        {

            if (!dr.Table.Columns.Contains(columnName) || dr.IsNull(columnName))
            {
                return defaultValue;
            }

            return Convert.ToInt32(dr[columnName]);
        }

        public static DateTime InferDate(this DataRow dr, string columnName)
        {
            if (!dr.Table.Columns.Contains(columnName) || dr.IsNull(columnName))
            {
                return DateTime.Today;
            }

            return Convert.ToDateTime(dr[columnName]);
        }

        public static string GetCSharpType(this DataRow dr)
        {
            bool isNullable = Convert.ToBoolean(dr["AllowDBNull"]);
            string type = dr["DataType"].ToString();

            switch (dr["DataType"].ToString())
            {
                case "System.Int16":
                    if (isNullable)
                    {
                        return "Nullable<short>";
                    }
                    else
                    {
                        return "short";
                    }
                case "System.Int32":
                    if (isNullable)
                    {
                        return "Nullable<int>";
                    }
                    else
                    {
                        return "int";
                    }
                case "System.String":
                    return "string";
                case "System.DateTime":
                    return "DateTime";
                case "System.Byte":
                    if (isNullable)
                    {
                        return "Nullable<byte>";
                    }
                    else
                    {
                        return "byte";
                    }
                case "System.Decimal":
                    if (isNullable)
                    {
                        return "Nullable<Decimal>";
                    }
                    else
                    {
                        return "Decimal";
                    }
                case "System.Byte[]":
                    return "Binary";
                case "System.Boolean":
                    if (isNullable)
                    {
                        return "Nullable<bool>";
                    }
                    else
                    {
                        return "bool";
                    }

                case "System.GUID":
                    if (isNullable)
                    {
                        return "Nullable<GUID>";
                    }
                    else
                    {
                        return "GUID";
                    }
                case "System.Guid":
                    if (isNullable)
                    {
                        return "Nullable<Guid>";
                    }
                    else
                    {
                        return "Guid";
                    }
                default:
                    return "dynamic";
                    //throw new Exception("Type not known");
            }
        }

        #region SqaDataReader

        public static string InferString(this SqlDataReader dr, string columnName)
        {
            bool exists = dr.IsColumnExist(columnName);

            if (exists)
            {
                int ordinal = dr.GetOrdinal(columnName);

                if (!dr.IsDBNull(ordinal))
                {
                    return dr[columnName].ToString();
                }
            }

            return string.Empty;
        }

        public static int InferInt(this SqlDataReader dr, string columnName)
        {
            bool exists = dr.IsColumnExist(columnName);

            if (exists)
            {
                int ordinal = dr.GetOrdinal(columnName);

                if (!dr.IsDBNull(ordinal))
                {
                    return Convert.ToInt32(dr[columnName]);
                }
            }

            return 0;
        }

        public static decimal InferDecimal(this SqlDataReader dr, string columnName)
        {
            bool exists = dr.IsColumnExist(columnName);

            if (exists)
            {
                int ordinal = dr.GetOrdinal(columnName);

                if (!dr.IsDBNull(ordinal))
                {
                    return Convert.ToDecimal(dr[columnName]);
                }
            }

            return 0;
        }

        public static bool InferBool(this SqlDataReader dr, string columnName)
        {
            bool exists = dr.IsColumnExist(columnName);

            if (exists)
            {
                int ordinal = dr.GetOrdinal(columnName);

                if (!dr.IsDBNull(ordinal))
                {
                    return Convert.ToBoolean(dr[columnName]);
                }
            }

            return false;
        }

        public static decimal? InferDecimal(this SqlDataReader dr, string columnName, decimal? defaultValue)
        {
            bool exists = dr.IsColumnExist(columnName);

            if (exists)
            {
                int ordinal = dr.GetOrdinal(columnName);

                if (!dr.IsDBNull(ordinal))
                {
                    return Convert.ToDecimal(dr[columnName]);
                }
            }

            return defaultValue;
        }

        public static int? InferInt(this SqlDataReader dr, string columnName, int? defaultValue)
        {
            bool exists = dr.IsColumnExist(columnName);

            if (exists)
            {
                int ordinal = dr.GetOrdinal(columnName);

                if (!dr.IsDBNull(ordinal))
                {
                    return Convert.ToInt32(dr[columnName]);
                }
            }

            return defaultValue;
        }

        public static DateTime InferDate(this SqlDataReader dr, string columnName)
        {
            bool exists = dr.IsColumnExist(columnName);

            if (exists)
            {
                int ordinal = dr.GetOrdinal(columnName);

                if (!dr.IsDBNull(ordinal))
                {
                    return Convert.ToDateTime(dr[columnName]);
                }
            }

            return DateTime.MinValue;
        }

        public static bool IsColumnExist(this SqlDataReader dr, string columnName)
        {
            return dr.GetSchemaTable().Rows.OfType<DataRow>().Any(x => x["ColumnName"].ToString() == columnName);
        }
      
        #endregion
    }
}
