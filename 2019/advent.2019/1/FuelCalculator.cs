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
            ulong fuelPlusTwo = mass / 3;
            return fuelPlusTwo > 2 ? fuelPlusTwo - 2 : 0;
        }

        private async ValueTask<ulong> SumFuel(
            IAsyncEnumerable<string> lines,
            Func<ulong, ulong> massToFuel)
        {
            ulong sum = 0;
            await foreach (string line in lines)
            {
                var mass = ulong.Parse(line);
                var fuel = massToFuel(mass);
                checked { sum += fuel; }
            }
            return sum;
        }

        public ValueTask<ulong> SumFuel(IAsyncEnumerable<string> lines) =>
            SumFuel(lines, MassToFuel);

        public ValueTask<ulong> SumFuel_Limit(IAsyncEnumerable<string> lines) =>
            SumFuel(lines, MassToFuel_Limit);

        public static ulong MassToFuel_Limit(ulong mass)
        {
            ulong totalFuel = 0;
            ulong addedMass = mass;
            while (addedMass > 0)
            {
                var fuel = MassToFuel(addedMass);
                totalFuel += fuel;
                addedMass = fuel;
            }
            return totalFuel;
        }
    }
}
