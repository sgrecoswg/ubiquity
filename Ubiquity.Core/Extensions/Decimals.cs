using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ubiquity.Core.Extensions
{
    public static class DecimalExtensions
    {
        /// <summary>
        /// To get formatted price and return nullable decimal
        /// </summary>
        /// <param name="price">decimal price</param>
        /// <returns>returns two decimal value</returns>
        public static decimal? FormatNullablePrice(decimal? price)
        {
            return Equals(price, null) ? 0 : Math.Round(price.Value, 2);
        }

        /// <summary>
        /// To get formatted price
        /// </summary>
        /// <param name="price">double price</param>
        /// <returns>returns string with two decimal value</returns>
        public static string FormatPrice(double? price)
        {
            return Equals(price, null) ? string.Empty : string.Format("{0:0.00}", price);
        }

        /// <summary>
        /// To get formatted price 
        /// </summary>
        /// <param name="price">decimal price</param>
        /// <returns>returns two decimal value</returns>
        public static decimal FormatPrice(decimal? price)
        {
            return Equals(price, null) ? 0 : Math.Round(price.Value, 2);
        }

    }
}
