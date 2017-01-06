using System;
using System.Collections.Generic;
using System.Linq;

namespace Ical.Net.Utility
{
    public static class CollectionHelpers
    {
        public static int GetHashCode<T>(IEnumerable<T> collection)
        {
            unchecked
            {
                return collection?.Where(e => e != null)
                    .Aggregate(397, (current, element) => current * 397 + element.GetHashCode()) ?? 0;
            }
        }

        public static bool Equals<T>(IEnumerable<T> left, IEnumerable<T> right, bool orderSignificant = false)
        {
            if (ReferenceEquals(left, right))
            {
                return true;
            }

            if (left == null && right != null)
            {
                return false;
            }

            if (left != null && right == null)
            {
                return false;
            }

            if (orderSignificant)
            {
                return left.SequenceEqual(right);
            }

            try
            {
                //Many things have natural IComparers defined, but some don't, because no natural comparer exists
                return left.OrderBy(l => l).SequenceEqual(right.OrderBy(r => r));
            }
            catch (Exception)
            {
                //It's not possible to sort some collections of things (like Calendars) in any meaningful way. Properties can be null, and there's no natural
                //ordering for the contents therein. In cases like that, the best we can do is treat them like sets, and compare them. We don't maintain
                //fidelity with respect to duplicates, but it seems better than doing nothing
                var leftSet = new HashSet<T>(left);
                var rightSet = new HashSet<T>(right);
                return leftSet.SetEquals(rightSet);
            }
        }

        public static IEnumerable<T> Clone<T>(IEnumerable<T> collection) where T : ICloneable
        {
            if (collection == null)
            {
                return null;
            }

            var sizedCollection = collection as ICollection<T>;
            if (sizedCollection != null)
            {
                var toReturn = new List<T>(sizedCollection.Count);
                var cloned = sizedCollection.Select(i => i.Clone()).Cast<T>().ToList();
                toReturn.AddRange(cloned);
                return toReturn;
            }
            return collection.Select(i => (T) i.Clone());
        }

        public static IEnumerable<TV> CloneStructs<TV>(IEnumerable<TV> collection) where TV : struct
        {
            if (collection == null)
            {
                return null;
            }

            var sizedCollection = collection as ICollection<TV>;
            if (sizedCollection != null)
            {
                var toReturn = new List<TV>(sizedCollection.Count);
                toReturn.AddRange(sizedCollection);
                return toReturn;
            }
            return collection.Select(i => i);
        }
    }
}
