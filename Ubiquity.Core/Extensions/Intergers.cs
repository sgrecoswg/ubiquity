using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ubiquity.Core.Extensions
{
    public static class IntExtensions
    {
        /// <summary>
        /// Indicate whether the number falls in the specified range.
        /// </summary>
        /// <param name="theNumber">Number to process</param>
        /// <param name="lower">Lower bound</param>
        /// <param name="upper">Upper bound</param>
        /// <returns>True/False</returns>
        public static bool Between(this int theNumber, int lower, int upper)
        {
            return theNumber >= lower && theNumber <= upper;
        }

        /// <summary>
        /// Indicate whether the number is even.
        /// </summary>
        /// <param name="theNumber">Number to process</param>
        /// <returns>True/False</returns>
        public static bool IsEven(this int theNumber)
        {
            return (theNumber % 2) == 0;
        }

        /// <summary>
        /// Returns the nearest power of 2 that is bigger than the number.
        /// </summary>
        /// <param name="theNumber">Number to process</param>
        /// <returns>Integer</returns>
        public static int RoundToPowerOf2(this int theNumber)
        {
            var exponent = 1;
            while (true)
            {
                var powerOf2 = (UInt32)Math.Pow(2, exponent++);
                if (powerOf2 >= theNumber)
                    return (int)powerOf2;
            }
        }

        /// <summary>
        /// Parse the number to a string or a default string if outside given range.
        /// </summary>
        /// <param name="theNumber">Number to process</param>
        /// <param name="upto">Lower bound</param>
        /// <param name="beyond">Upper bound</param>
        /// <param name="defaultText">Default text</param>
        /// <returns>String</returns>
        public static string ToStringOrEmpty(this int theNumber, int upto = 0, int beyond = 9000, string defaultText = "")
        {
            return theNumber <= upto || theNumber > beyond
                ? defaultText
                : theNumber.ToString(CultureInfo.InvariantCulture);
        }

        /// <summary>
        /// Decrement a number ensuring it never passes a given lower-bound.
        /// </summary>
        /// <param name="number">Number to process</param>
        /// <param name="lowerBound">Lower bound</param>
        /// <param name="step">Step of the decrement</param>
        /// <returns>Integer</returns>
        public static int Decrement(this int number, int lowerBound = 0, int step = 1)
        {
            var n = number - step;
            return n < lowerBound ? lowerBound : n;
        }

        /// <summary>
        /// Increment a number ensuring it never passes a given upper-bound.
        /// </summary>
        /// <param name="number">Number to process</param>
        /// <param name="upperBound">Upper bound</param>
        /// <param name="step">Step of the increment</param>
        /// <returns>Integer</returns>
        public static int Increment(this int number, int upperBound = 100, int step = 1)
        {
            var n = number + 1;
            return n > upperBound ? upperBound : n;
        }

        /// <summary>
        /// Translate the number in words (English)
        /// </summary>
        /// <param name="number">Number to translate</param>
        /// <returns>String</returns>
        public static string ToWords(this int number)
        {
            if (number == 0)
                return "Zero";

            if (number < 0)
                return "Minus " + ToWords(Math.Abs(number));

            var words = "";

            if ((number / 1000000) > 0)
            {
                words += ToWords(number / 1000000) + " million ";
                number %= 1000000;
            }

            if ((number / 1000) > 0)
            {
                words += ToWords(number / 1000) + " Thousand ";
                number %= 1000;
            }

            if ((number / 100) > 0)
            {
                words += ToWords(number / 100) + " Hundred ";
                number %= 100;
            }

            if (number > 0)
            {
                if (words != "")
                    words += "and ";

                var units = new[] { "Zero", "One", "Two", "Three", "Four", "Five", "Six", "Seven", "Eight", "Nine", "Ten", "Eleven", "Twelve", "Thirteen", "Fourteen", "Fifteen", "Sixteen", "Seventeen", "Eighteen", "Nineteen" };
                var tens = new[] { "Zero", "Ten", "Twenty", "Thirty", "Forty", "Fifty", "Sixty", "Seventy", "Eighty", "Ninety" };

                if (number < 20)
                    words += units[number];
                else
                {
                    words += tens[number / 10];
                    if ((number % 10) > 0)
                        words += "-" + units[number % 10];
                }
            }

            return words;
        }
    }
}
