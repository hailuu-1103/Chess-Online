namespace Ultility
{
    using System.Collections.Generic;
    using System.Linq;

    public static class ListExtensions
    {
        public static bool AllElementsNotExistInList<T>(List<T> listA, List<T> listB)
        {
            return listA.All(element => !listB.Contains(element));
        }

        public static List<T> GetIntersectList<T>(List<T> listA, List<T> listB)
        {
            return listA.Intersect(listB).ToList();
        }
    }
}