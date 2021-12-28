using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ubiquity.Core.Extensions
{
    public static class CollectionExtension
    {
        /// <summary>
        /// Converts a List objevt to a Collection object.
        /// </summary>
        /// <typeparam name="T">An object in a list.</typeparam>
        /// <param name="items">The List object we want to convert</param>
        /// <returns></returns>
        public static Collection<T> ListToCollection<T>(this List<T> items)
        {
            Collection<T> collection = new Collection<T>();

            for (int i = 0; i < items.Count; i++)
            {
                collection.Add(items[i]);
            }

            return collection;
        }
    }
}
