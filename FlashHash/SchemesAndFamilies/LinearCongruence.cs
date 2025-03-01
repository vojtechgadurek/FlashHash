﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FlashHash.Utils;

namespace FlashHash.SchemesAndFamilies
{

    public record class LinearCongruenceFamily : IHashFunctionFamily<LinearCongruenceScheme>
    {
        Random? _random = new Random();


        public LinearCongruenceScheme GetScheme(ulong size, ulong offset)
        {
            return new LinearCongruenceScheme((ulong)_random!.Next(), (ulong)_random.Next(), size, offset);
        }

        public void SetRandomness(Random random)
        {
            _random = random;
        }

        IHashFunctionScheme IHashFunctionFamily.GetScheme(ulong size, ulong offset)
        {
            return GetScheme(size, offset);
        }
    }

    public readonly record struct LinearCongruenceScheme(ulong Multiply, ulong Add, ulong Size, ulong Offset) : IHashFunctionScheme
    {
        static List<int> MersennePrimesExponents = new List<int>
        {
            2,
            3,
            5,
            7,
            13,
            17,
            19,
            31,
            61,
        };

        static List<ulong> MersennePrimes = new List<ulong> { };

        static LinearCongruenceScheme()
        {
            foreach (var exponent in MersennePrimesExponents)
            {
                MersennePrimes.Add((2UL << exponent) - 1UL);
            }
        }
        public static ulong GetGoodPrime(ulong size)
        {
            var index = MersennePrimes.FindIndex(x => x > size);
            if (index == -1)
            {
                throw new InvalidOperationException("Size is larger than 2^61");
            }
            return MersennePrimes[index];
        }

        public Expression<HashingFunction> Create()
        {
            var f = new CompiledFunction<ulong, ulong>(out var value);
            f.S.Assign(f.Output, (value.V * Multiply % GetGoodPrime(Size) + Add) % Size + Offset);
            return f.Construct();
        }

        public bool Equals(IHashFunctionScheme? other)
        {
            if (other is LinearCongruenceScheme lcs)
            {
                return lcs.Multiply == Multiply && lcs.Add == Add && lcs.Size == Size && lcs.Offset == Offset;
            }
            return false;
        }
    }
}