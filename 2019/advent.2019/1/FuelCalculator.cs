using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace advent._2019._1
{
    public readonly struct FuelCalculator
    {
        public static ulong MassToFuel(ulong mass)
        {
            return checked((mass / 3) - 2);
        }

        public async ValueTask<ulong> SumFuel(IAsyncEnumerable<string> lines)
        {
            ulong sum = 0;
            await foreach (string line in lines)
            {
                var mass = ulong.Parse(line);
                var fuel = MassToFuel(mass);
                checked { sum += fuel; }
            }
            return sum;
        }
    }
}
