using System;
using System.Collections.Generic;
using System.Text;

namespace advent._2019
{
    public static class ArrayExtensions
    {
        public delegate void RefAction<T>(ref T x);
        public static void Foreach<T>(this T[,] array, RefAction<T> f)
        {
            for (int y = 0; y < array.GetLength(0); y++)
            {
                for (int x = 0; x < array.GetLength(1); x++)
                {
                    f(ref array[y, x]);
                }
            }
        }
    }
}
