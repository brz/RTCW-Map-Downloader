using System;
using System.Collections.Generic;

namespace RTCWMapDownloader
{
    public static class Extensions
    {
        public static T PickRandom<T>(this List<T> list)
        {
            if (list.Count == 0)
                return default(T);
            if (list.Count == 1)
                return list[0];
            Random rnd = new Random(DateTime.Now.Millisecond);
            return list[(rnd.Next(0, list.Count))];
        }
    }
}
