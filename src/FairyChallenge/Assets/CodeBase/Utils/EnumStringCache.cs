using System;
using System.Collections.Generic;
using UnityEngine;

namespace Savidiy.Utils
{
    public class EnumStringCache<T> where T : Enum
    {
        private readonly Dictionary<T, string> _cache = new();

        public EnumStringCache()
        {
            foreach (T value in Enum.GetValues(typeof(T)))
            {
                _cache[value] = value.ToString();
            }
        }

        public string ToString(T value)
        {
            if (_cache.TryGetValue(value, out string result))
                return result;

            Debug.LogError("Can't find value in cache");
            return string.Empty;
        }
    }
}