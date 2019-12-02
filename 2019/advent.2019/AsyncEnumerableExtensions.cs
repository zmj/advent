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
                return enumerator.Current;
            }
            finally
            {
                await enumerator.DisposeAsync();
            }
        }
    }
}
