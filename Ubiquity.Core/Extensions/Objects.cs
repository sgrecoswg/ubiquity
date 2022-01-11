using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using System.Collections.Generic;
using System.Globalization;
using System.Reflection;
using System.Xml.Linq;
using System.Linq.Expressions;
using System.Dynamic;
using System.Runtime.CompilerServices;
using Microsoft.CSharp.RuntimeBinder;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;


using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Ubiquity.Core.Extensions
{
    public static class ObjectExtensions
    {
        /// <summary>
        /// Copies property values that match in name to an object
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="self"></param>
        /// <param name="parent"></param>
        /// <returns></returns>
        public static T From<T>(this T self, object parent)
        {
            var fromProperties = parent.GetType().GetProperties();
            var toProperties = self.GetType().GetProperties();

            foreach (var fromProperty in fromProperties)
            {
                foreach (var toProperty in toProperties)
                {
                    if (fromProperty.Name == toProperty.Name && fromProperty.PropertyType == toProperty.PropertyType)
                    {
                        toProperty.SetValue(self, fromProperty.GetValue(parent));
                        break;
                    }
                }
            }
            return self;
        }

        /// <summary>
        /// Copies the properties from one object to the other
        /// </summary>
        /// <param name="source">The source object</param>
        /// <param name="destination">Where the properties are going</param>
        /// <param name="skipTheseProps">properties we don't want to copy</param>
        public static void CopyPropertiesTo(this object source, object destination, params string[] skipTheseProps)
        {
            // If source is null this will throw an exception
            if (source == null || destination == null)
                throw new Exception("Source or/and Destination Objects are null");
            // Getting the Types of the objects
            Type typeDest = destination.GetType();
            Type typeSrc = source.GetType();

            //Get properties from source
            PropertyInfo[] srcProps = typeSrc.GetProperties();
            foreach (PropertyInfo srcProp in srcProps)
            {
                if (!srcProp.CanRead)
                {
                    continue;
                }
                PropertyInfo targetProperty = typeDest.GetProperty(srcProp.Name);
                if (targetProperty == null)
                {
                    continue;
                }
                if (!targetProperty.CanWrite)
                {
                    continue;
                }
                if (targetProperty.GetSetMethod(true) != null && targetProperty.GetSetMethod(true).IsPrivate)
                {
                    continue;
                }
                if ((targetProperty.GetSetMethod().Attributes & MethodAttributes.Static) != 0)
                {
                    continue;
                }
                if (!targetProperty.PropertyType.IsAssignableFrom(srcProp.PropertyType))
                {
                    continue;
                }
                //
                if (skipTheseProps.Contains(srcProp.Name, StringComparer.CurrentCultureIgnoreCase))
                {
                    continue;
                }
                // If it passes all tests, set the value
                targetProperty.SetValue(destination, srcProp.GetValue(source, null), null);
            }
        }

        /// <summary>
        /// Get a proepry from an object
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public static object GetProperty(this object obj, string name)
        {
            object propValue = null;
            PropertyInfo propInfo = obj.GetType().GetProperty(name);
            if (!Equals(propInfo, null))
            {
                propValue = propInfo.GetValue(obj, null);
            }
            return propValue;
        }

        /// <summary>
        /// Sets the value of a property
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="propertyName"></param>
        /// <param name="value"></param>
        public static void SetPropertyValue(this object obj, string propertyName, object value)
        {
            PropertyInfo propInfo = obj.GetType().GetProperty(propertyName);
            if (!Equals(propInfo, null))
            {
                propInfo.SetValue(obj, value);
            }
        }

        /// <summary>
        /// Tries to get a value feom an element in a node
        /// </summary>
        /// <param name="parentEl"></param>
        /// <param name="elementName"></param>
        /// <param name="defaultValue"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public static string TryGetElementValue(this XElement parentEl, string elementName, string defaultValue = null, string type = null)
        {
            var foundEl = parentEl.Element(elementName);
            string result = string.Empty;
            if (!Equals(foundEl, null))
            {
                result = foundEl.Value;
                if (string.IsNullOrEmpty(result.Trim()))
                {
                    if (Equals(type, "Char"))
                    {
                        return "n";
                    }
                    else if (Equals(type, "Int"))
                    {
                        return "0";
                    }
                }
                return result;
            }

            if (Equals(type, "Char"))
            {
                return "n";
            }
            else if (Equals(type, "Int"))
            {
                return "0";
            }
            return defaultValue;
        }

        /// <summary>
        /// Gets a property from an object
        /// </summary>
        /// <param name="o"></param>
        /// <param name="member"></param>
        /// <returns></returns>
        public static object GetDynamicProperty(this object o, string member)
        {
            try
            {
                if (o == null) throw new ArgumentNullException("o");
                if (member == null) throw new ArgumentNullException("member");
                Type scope = o.GetType();
                IDynamicMetaObjectProvider provider = o as IDynamicMetaObjectProvider;
                if (provider != null)
                {
                    ParameterExpression param = Expression.Parameter(typeof(object));
                    DynamicMetaObject mobj = provider.GetMetaObject(param);
                    GetMemberBinder binder = (GetMemberBinder)Microsoft.CSharp.RuntimeBinder.Binder.GetMember(0, member, scope, new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(0, null) });
                    DynamicMetaObject ret = mobj.BindGetMember(binder);
                    BlockExpression final = Expression.Block(
                        Expression.Label(CallSiteBinder.UpdateLabel),
                        ret.Expression
                    );
                    LambdaExpression lambda = Expression.Lambda(final, param);
                    Delegate del = lambda.Compile();
                    return del.DynamicInvoke(o);
                }
                else
                {
                    return o.GetType().GetProperty(member, BindingFlags.Public | BindingFlags.Instance).GetValue(o, null);
                }

            }
            catch (Exception)
            {
                return null;
            }
        }

        /// <summary>
        /// Gets the specified attribute from the PropertyDescriptor.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="prop"></param>
        /// <returns></returns>
        public static T GetAttribute<T>(this PropertyDescriptor prop) where T : Attribute
        {
            foreach (Attribute att in prop.Attributes)
            {
                var tAtt = att as T;
                if (tAtt != null) return tAtt;
            }
            return null;
        }

        /// <summary>
        /// Gets the specified attribute from the type.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="type"></param>
        /// <param name="inherit"></param>
        /// <returns></returns>
        public static T GetAttribute<T>(this Type type, bool inherit) where T : Attribute
        {
            var atts = type.GetCustomAttributes(typeof(T), inherit);
            if (atts.Length == 0) return null;
            return atts[0] as T;
        }

        /// <summary>
        /// Gets the specified attribute for the assembly.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="asm"></param>
        /// <returns></returns>
        public static T GetAttribute<T>(this Assembly asm) where T : Attribute
        {
            if (asm == null) return null;

            var atts = asm.GetCustomAttributes(typeof(T), false);
            if (atts == null) return null;
            if (atts.Length == 0) return null;

            return (T)atts[0];
        }

        /// <summary>
        /// Gets the specified attribute from the PropertyDescriptor.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <param name="inherit"></param>
        /// <returns></returns>
        public static T GetAttribute<T>(this object obj, bool inherit) where T : Attribute
        {
            if (obj == null) return null;
            return obj.GetType().GetAttribute<T>(inherit);
        }
               

        /// <summary>
        /// Turns a list of T to a data table
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="data"></param>
        /// <returns></returns>
        public static DataTable ToDataTable<T>(this IList<T> data)
        {
            var table = new DataTable();
            if (data.Count() > 0)
            {
                var properties = TypeDescriptor.GetProperties(typeof(T));
                
                foreach (PropertyDescriptor prop in properties)
                {
                    // = Attribute.IsDefined(prop, typeof(IgnoreAttribute));
                    var ignoreThis = prop.Attributes.OfType<IgnoreAttribute>().Any();
                    if (!ignoreThis)
                    {
                        var hasAlternateName = prop.Attributes.OfType<DisplayAttribute>().Any();
                        if (hasAlternateName)
                        {
                            var alternateName = prop.GetAttribute<DisplayAttribute>().Name;
                            table.Columns.Add(alternateName ?? prop.Name, Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType);
                        }
                        else
                        {
                            table.Columns.Add(prop.Name, Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType);
                        }
                    }
                }

                foreach (T item in data)
                {
                    var row = table.NewRow();
                    foreach (PropertyDescriptor prop in properties)
                    {
                        var ignoreThis = prop.Attributes.OfType<IgnoreAttribute>().Any();
                        if (!ignoreThis)
                        {
                            var hasAlternateName = prop.Attributes.OfType<DisplayAttribute>().Any();
                            if (hasAlternateName)
                            {
                                var alternateName = prop.GetAttribute<DisplayAttribute>().Name;
                                row[alternateName ?? prop.Name] = prop.GetValue(item) ?? DBNull.Value;
                            }
                            else
                            {
                                row[prop.Name] = prop.GetValue(item) ?? DBNull.Value;
                            }
                        }

                    }
                    table.Rows.Add(row);
                }

            }
            return table;
        }

        /// <summary>
        /// checks to see if this object inherits IEnumerable
        /// </summary>
        /// <param name="myProperty"></param>
        /// <returns></returns>
        public static bool IsEnumerable(this object myProperty)
        {
            if (typeof(IEnumerable<>).IsAssignableFrom(myProperty.GetType())
                || typeof(IEnumerable<>).IsAssignableFrom(myProperty.GetType()))
                return true;

            return false;
        }

        /// <summary>
        /// checks to see if this object inherits IEnumerable
        /// </summary>
        /// <param name="myProperty"></param>
        /// <returns></returns>
        public static bool IsInheriting<T>(this object obj, T item)
        {
            if (typeof(T).IsAssignableFrom(obj.GetType()) || typeof(T).IsAssignableFrom(obj.GetType()))
                return true;

            return false;
        }

        /// <summary>
        /// checks to see if the list of objects has items in it.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="items"></param>
        /// <returns></returns>
        public static bool HasItems<T>(this IQueryable<T> items)
        {
            return items != null && items.Count() > 0;
        }

        /// <summary>
        /// A check to see if it's null.
        /// </summary>
        /// <comment>May not help optimize, but makes it more readable. Use your best judgement</comment>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static bool IsNull(this object obj)
        {
            return obj == null;
        }

#if NET5_0
        [Obsolete("You dont need to use this anymore, please use 'is not null' instead ")]
#endif
        /// <summary>
        /// A check to see if something is not null
        /// </summary>
        ///<comment>May not help optimize, but makes it more readable. Use your best judgement</comment>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static bool IsNotNull(this object obj)
        {
            return obj != null;
        }

        /// <summary>
        /// Tries to comvert an object to a string that represents a DateTime
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        private static string ConvertToDateString(object date)
        {
            if (date == null)
                return string.Empty;

            return date == null ? string.Empty : Convert.ToDateTime(date).ConvertDate();
        }

        /// <summary>
        /// Turns an object to a string
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        private static string ConvertToString(object value)
        {
            return Convert.ToString(ReturnEmptyIfNull(value));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        private static int ConvertToInt(object value)
        {
            return Convert.ToInt32(ReturnZeroIfNull(value));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        private static long ConvertToLong(object value)
        {
            return Convert.ToInt64(ReturnZeroIfNull(value));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        private static decimal ConvertToDecimal(object value)
        {
            return Convert.ToDecimal(ReturnZeroIfNull(value));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        private static DateTime ConvertToDateTime(object date)
        {
            return Convert.ToDateTime(ReturnDateTimeMinIfNull(date));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static object ReturnEmptyIfNull(this object value)
        {
            if (value == DBNull.Value)
                return string.Empty;
            if (value == null)
                return string.Empty;
            return value;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static object ReturnZeroIfNull(this object value)
        {
            if (value == DBNull.Value)
                return 0;
            if (value == null)
                return 0;
            return value;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static object ReturnDateTimeMinIfNull(this object value)
        {
            if (value == DBNull.Value)
                return DateTime.MinValue;
            if (value == null)
                return DateTime.MinValue;
            return value;
        }

        /// <summary>
        /// Turns a DataTable into a List of objects T
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="tbl"></param>
        /// <returns></returns>
        public static List<T> ToListOf<T>(this DataTable tbl) where T : new()
        {
            // define return list
            List<T> lst = new List<T>();

            // go through each row
            foreach (DataRow r in tbl.Rows)
            {
                // add to the list
                lst.Add(CreateItemFromRow<T>(r));
            }

            // return the list
            return lst;
        }

        /// <summary>
        /// Function that creates an object from the given data row
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="row"></param>
        /// <returns></returns>
        public static T CreateItemFromRow<T>(DataRow row) where T : new()
        {
            // create a new object
            T item = new T();

            // set the item
            SetItemFromRow(item, row);

            // return 
            return item;
        }

        /// <summary>
        /// Turns a row into an object.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="row"></param>
        /// <returns></returns>
        public static T ToClass<T>(this DataRow row) where T : new()
        {
            return CreateItemFromRow<T>(row);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="item"></param>
        /// <param name="row"></param>
        public static void SetItemFromRow<T>(T item, DataRow row) where T : new()
        {
            var properties = TypeDescriptor.GetProperties(typeof(T));

            Dictionary<string, string> _propertyNameConversions = new Dictionary<string, string>();
            for (int i = 0; i < properties.Count; i++)
            {
                string _columnName;
                var _currentProp = properties[i];
                var hasColumnAtttribute = _currentProp.Attributes.OfType<ColumnAttribute>().Any();
                if (hasColumnAtttribute)
                {
                    ColumnAttribute _att = _currentProp.GetAttribute<ColumnAttribute>();
                    _columnName = _att.Name;
                    _propertyNameConversions.Add(_columnName, _currentProp.Name);
                }
                else
                {
                    _columnName = _currentProp.Name;
                    _propertyNameConversions.Add(_columnName, _columnName);
                }
            }

            // go through each column
            foreach (DataColumn c in row.Table.Columns)
            {
                string columnMappedName = string.Empty;
                bool _hasKey = _propertyNameConversions.TryGetValue(c.ColumnName, out columnMappedName);
                if (_hasKey)
                {
                    // find the property for the column
                    PropertyInfo p = item.GetType().GetProperty(columnMappedName);

                    // if exists, set the value
                    if (p != null && row[c] != DBNull.Value)
                    {
                        if (p.PropertyType == typeof(DateTime))
                        {
                            p.SetValue(item, ConvertToDateTime(row[c]), null);
                        }
                        else if (p.PropertyType == typeof(DateTime?))
                        {
                            p.SetValue(item, ConvertToDateTime(row[c]), null);
                        }
                        else if (p.PropertyType == typeof(int))
                        {
                            p.SetValue(item, ConvertToInt(row[c]), null);
                        }
                        else if (p.PropertyType == typeof(long))
                        {
                            p.SetValue(item, ConvertToLong(row[c]), null);
                        }
                        else if (p.PropertyType == typeof(decimal))
                        {
                            p.SetValue(item, ConvertToDecimal(row[c]), null);
                        }
                        else if (p.PropertyType == typeof(String))
                        {
                            if (row[c].GetType() == typeof(DateTime))
                            {
                                p.SetValue(item, ConvertToDateString(row[c]), null);
                            }
                            else
                            {
                                p.SetValue(item, ConvertToString(row[c]), null);
                            }
                        }
                        else
                        {
                            p.SetValue(item, Convert.ChangeType(row[c], p.PropertyType), null);
                        }
                    }
                }
            }
        }
    }
}
