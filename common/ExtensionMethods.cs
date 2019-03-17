using System;
using System.Collections.Generic;

namespace SiobhanDev
{
    public static class ExtensionMethods
    {
        public static Dictionary<string, string> Fold(this string[] array)
        {
            Dictionary<string, string> kvps = new Dictionary<string, string>();
            for (int i = 0; i < (array.Length - 1); i += 2)
            {
                kvps.Add(array[i], array[i + 1]);
            }
            return kvps;
        }

        public static string[] Unfold(this Dictionary<string, string> kvps)
        {
            var result = new string[kvps.Count * 2];

            int index = 0;
            foreach (var kvp in kvps)
            {
                result[index] = kvp.Key;
                result[index + 1] = kvp.Value;
                index += 2;
            }

            return result;
        }
    }
}