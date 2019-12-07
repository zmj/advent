using System;
using System.Collections.Generic;
using System.Text;

namespace advent._2019
{
    public static class Permutations
    {
        public static IEnumerable<T[]> Enumerate<T>(params T[] values) => Generate(values.Length, values);

        // https://en.wikipedia.org/wiki/Heap%27s_algorithm
        private static IEnumerable<T[]> Generate<T>(int k, T[] values)
        {
            if (k == 1)
            {
                yield return values;
                yield break;
            }

            foreach (var permutation in Generate(k - 1, values))
            {
                yield return permutation;
            }

            for (int i = 0; i < k - 1; i++)
            {
                if (k % 2 == 0)
                {
                    (values[i], values[k - 1]) = (values[k - 1], values[i]);
                }
                else
                {
                    (values[0], values[k - 1]) = (values[k - 1], values[0]);
                }
                foreach (var permutation in Generate(k - 1, values))
                {
                    yield return permutation;
                }
            }
        }
    }
}
