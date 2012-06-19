using System;
using System.Threading;

namespace Stixter.Plexi.Core
{
    public static class RandomHelper
    {
        private static int _seedCounter = new Random().Next();

        [ThreadStatic]
        private static Random _random;

        public static Random Instance
        {
            get
            {
                if (_random == null)
                {
                    int seed = Interlocked.Increment(ref _seedCounter);
                    _random = new Random(seed);
                }
                return _random;
            }
        }
    }
}