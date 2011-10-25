﻿using System;
using System.Threading;

public static class RandomHelper
{
    private static int seedCounter = new Random().Next();

    [ThreadStatic]
    private static Random rng;

    public static Random Instance
    {
        get
        {
            if (rng == null)
            {
                int seed = Interlocked.Increment(ref seedCounter);
                rng = new Random(seed);
            }
            return rng;
        }
    }
}