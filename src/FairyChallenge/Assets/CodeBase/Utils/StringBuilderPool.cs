using System.Collections.Generic;
using System.Text;

namespace Savidiy.Utils
{
    public static class StringBuilderPool
    {
        private static readonly List<StringBuilder> Pool = new();

        public static StringBuilder Get()
        {
            if (Pool.Count == 0)
                return new StringBuilder();

            int lastIndex = Pool.Count - 1;
            StringBuilder stringBuilder = Pool[lastIndex];
            Pool.RemoveAt(lastIndex);
            return stringBuilder;
        }
        
        public static void Release(StringBuilder stringBuilder)
        {
            stringBuilder.Clear();
            Pool.Add(stringBuilder);
        }
    }
}