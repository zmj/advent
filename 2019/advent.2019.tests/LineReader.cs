using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace advent._2019.tests
{
    public static class LineReader
    {
        public static IAsyncEnumerable<string> Open(string inputFile)
        {
            var path = @"../../../input/" + inputFile;
            return Lines(new StreamReader(path));
        }

        public static IAsyncEnumerable<string> Split(string s)
        {
            return Lines(new StringReader(s));
        }

        private static async IAsyncEnumerable<string> Lines(TextReader rdr)
        {
            using (rdr)
            {
                while (true)
                {
                    string? s = await rdr.ReadLineAsync();
                    if (s == null) { yield break; }
                    yield return s;
                }
            }
        }
    }
}
