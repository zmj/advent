using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace advent._2019._12
{
    public class MoonMap
    {
        private ImmutableArray<Moon> _moons;

        public void Step(int n)
        {
            for (int i=0; i<n; i++) { Step(); }
        }

        public void Step()
        {
            Span<Moon> newMoons = stackalloc Moon[_moons.Length];
            for (int i = 0; i < _moons.Length; i++)
            {
                var moon = _moons[i];
                for (int j = 0; j < _moons.Length; j++)
                {
                    if (i == j) { continue; }
                    moon = moon.AttractTo(_moons[j]);
                }
                newMoons[i] = moon;
            }

            for (int i = 0; i < newMoons.Length; i++)
            {
                newMoons[i] = newMoons[i].Move();
            }

            var builder = ImmutableArray.CreateBuilder<Moon>(newMoons.Length);
            for (int i = 0; i < newMoons.Length; i++) { builder.Add(newMoons[i]); }
            _moons = builder.ToImmutable();
        }

        public int Energy() => checked(_moons.Sum(m => m.Energy()));

        public async ValueTask<MoonMap> Load(IAsyncEnumerable<string> lines)
        {
            var moons = ImmutableArray.CreateBuilder<Moon>();
            await foreach (var line in lines)
            {
                moons.Add(new Moon(new Position(line), default));
            }
            _moons = moons.ToImmutable();
            return this;
        }
    }
}
