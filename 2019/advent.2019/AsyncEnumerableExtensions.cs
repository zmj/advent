using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace advent._2019
{
    internal static class AsyncEnumerableExtensions
    {
        public static async ValueTask<T> Single<T>(this IAsyncEnumerable<T> enumerable)
        {
            var enumerator = enumerable.GetAsyncEnumerator();
            try
            {
                if (!(await enumerator.MoveNextAsync()))
                {
                    throw new InvalidOperationException();
                }
                var value = enumerator.Current;
                if (await enumerator.MoveNextAsync())
                {
                    throw new InvalidOperationException();
                }
                return value;
            }
            finally
            {
                await enumerator.DisposeAsync();
            }
        }

        public static async IAsyncEnumerable<U> Select<T, U>(
            this IAsyncEnumerable<T> enumerable,
            Func<T, U> f)
        {
            await foreach (var t in enumerable)
            {
                yield return f(t);
            }
        }

        public static async ValueTask<T[]> ToArray<T>(
            this IAsyncEnumerable<T> enumerable)
        {
            var array = Array.Empty<T>();
            void GrowArray()
            {
                if (array.Length == 0)
                {
                    array = new T[16];
                    return;
                }
                ResizeArray(array.Length * 2);
            }

            void ResizeArray(int newSize)
            {
                var newArray = new T[newSize];
                array.AsSpan().Slice(0, newSize)
                    .CopyTo(newArray.AsSpan());
                array = newArray;
            }

            int i = 0;
            await foreach (var t in enumerable)
            {
                if (i >= array.Length) { GrowArray(); }
                array[i++] = t;
            }
            
            if (i < array.Length) { ResizeArray(i); }
            return array;
        }
    }
}
