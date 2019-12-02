using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace advent._2019.tests
{
    public static class LineReader
    {
        public static async IAsyncEnumerable<string> Open(string inputFile)
        {
            var path = @"../../../input/" + inputFile;
            using var reader = new StreamReader(path);
            while (true)
            {
                string? s = await reader.ReadLineAsync();
                if (s == null) { yield break; }
                yield return s;
            }
        }
    }
}
