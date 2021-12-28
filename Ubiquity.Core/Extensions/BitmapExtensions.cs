using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;

namespace Ubiquity.Core.Extensions
{
    public static class BitmapExtensions
    {
        public static IEnumerable<IEnumerable<T>> Partition<T>(this IEnumerable<T> instance,int size)
        {
            return instance.Select((value, index) => new { Index = index, Value = value })
                           .GroupBy(i => i.Index / size).Select(i => i.Select(i2 => i2.Value));
        }

        public static T[] GetColumn<T>(this T[,] matrix, int cindx)
        {
            return Enumerable.Range(0, matrix.GetLength(0)).Select(x => matrix[x, cindx]).ToArray();
        }

        public static T[] GetRow<T>(this T[,] matrix, int rowNumber)
        {
            return Enumerable.Range(0, matrix.GetLength(0)).Select(x => matrix[rowNumber, x]).ToArray();
        }

        public static T[,] GetMatrix<T>(this List<T> list)        
        {
            double columnCount = Math.Truncate(Math.Sqrt(list.Count()));
            double rowCount = Math.Ceiling(list.Count() / columnCount);
            T[,] matrix = new T[(int)columnCount, (int)rowCount];
            int rowCounter = 0, columncounter = 0;

            foreach (var item in list)
            {
                matrix.SetValue(item, columncounter, rowCounter);
                if (columncounter<columnCount-1)
                {
                    columncounter++;
                }
                else
                {
                    columncounter = 0;
                    rowCounter++;
                }
            }

            return matrix;

        }

        public static Size GetSize(this Bitmap[,] matrix, int padding = 0)
        {

            int calcWidth = 0;
            int calcHieght = 0;
            int[] columnWidthTtls = new int[matrix.GetLength(1)];

            for (int i = 0; i < matrix.GetLength(0); i+=1)
            {
                var currentRow = matrix.GetRow(i);
                calcHieght += currentRow.Where(b => b != null).Max(h => h.Height);
                columnWidthTtls[i] = currentRow.Where(b => b != null).Sum(r => r.Width);
            }
            calcWidth = columnWidthTtls.Max();

            if (padding > 0)
            {
                calcWidth += (padding * 2) * matrix.GetLength(0);
                calcHieght += (padding * 2) * matrix.GetLength(1);
            }

            return new Size(calcWidth,calcHieght);
        }

        public static Bitmap ToBitmap(this MemoryStream stream)
        {
            return new Bitmap(stream);
        }

        public static byte[] ToBytes(this Bitmap b)
        {
            return (byte[])(new ImageConverter().ConvertTo(b, typeof(byte[])));
        }
    }
}
