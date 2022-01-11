using System;
using System.Reflection;
using System.ComponentModel;

namespace Ubiquity.Core.Extensions
{
    public static class EnumExtensions
    {
        /// <summary>
        /// Gets the specified attribute for the assembly.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="val"></param>
        /// <returns></returns>
        public static T GetAttribute<T>(this Enum val) where T : Attribute
        {
            var fi = val.GetType().GetField(val.ToString());
            var atts = fi.GetCustomAttributes(typeof(T), false);
            if (atts.Length == 0) return null;
            return (T)atts[0];
        }

        /// <summary>
        /// Retrieve the description on the <paramref name="en"/> enum, e.g.
        /// <para>[Description("Bright Pink")]</para>
        /// <para>BrightPink = 2,</para>
        /// Then when you pass in the enum, it will retrieve the description as a  <see cref="String"/>
        /// </summary>
        /// <param name="en">The Enumeration</param>        
        /// <returns>A string representing the friendly name</returns>  
        public static string GetDescription(this Enum en)
        {
            Type type = en.GetType();

            MemberInfo[] memInfo = type.GetMember(en.ToString());

            if (memInfo != null && memInfo.Length > 0)
            {
                object[] attrs = memInfo[0].GetCustomAttributes(typeof(DescriptionAttribute), false);

                if (attrs != null && attrs.Length > 0)
                {
                    return ((DescriptionAttribute)attrs[0]).Description;
                }
            }

            return en.ToString();
        }
    }
}
