﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace advent._2019._6
{
    public class OrbitMap
    {
        private readonly Dictionary<string, string> _childToParent =
            new Dictionary<string, string>();

        public void Insert(string parent, string child)
        {
            if (!_childToParent.TryAdd(child, parent))
            {
                throw new Exception($"already added {child}");
            }
        }

        public int IndirectOrbits(string c) => c switch
        {
            "COM" => -1,
            _ => 1 + IndirectOrbits(_childToParent[c]),
        };

        public int TotalOrbits() => _childToParent.Keys.Sum(TotalOrbits);

        public int TotalOrbits(string c) => 1 + IndirectOrbits(c);

        public static (string, string) ParseOrbit(string line)
        {
            int i = line.IndexOf(')');
            return (line[..i], line[(i + 1)..]);
        }

        public async ValueTask<OrbitMap> Load(IAsyncEnumerable<string> lines)
        {
            await foreach (string line in lines)
            {
                var (parent, child) = ParseOrbit(line);
                Insert(parent, child);
            }
            return this;
        }
    }
}
