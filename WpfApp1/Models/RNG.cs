using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp1.Models
{
    public static class RNG
    {
        private static readonly Random _random = new Random();
        public static int Next(int min, int max) => _random.Next(min, max);
        public static double NextDouble() => _random.NextDouble();
        public static bool RollChance(int percent) => _random.Next(1, 101) <= percent;
    }
}
