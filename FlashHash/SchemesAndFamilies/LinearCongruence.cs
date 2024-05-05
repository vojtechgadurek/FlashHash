using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FlashHash.Utils;

namespace FlashHash.SchemesAndFamilies
{

	public record class LinearCongruenceFamily : IHashingFunctionFamily<LinearCongruenceScheme>
	{
		Random? _random = new Random();


		public LinearCongruenceScheme GetScheme(ulong size)
		{
			return new LinearCongruenceScheme((ulong)_random!.Next(), (ulong)_random.Next(), size);
		}

		public void SetRandomness(Random random)
		{
			_random = random;
		}

		IHashingFunctionScheme IHashingFunctionFamily.GetScheme(ulong size)
		{
			return GetScheme(size);
		}
	}

	public record struct LinearCongruenceScheme(ulong Multiply, ulong Add, ulong Size) : IHashingFunctionScheme
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
			f.S.Assign(f.Output, (value.V * Multiply % GetGoodPrime(Size) + Add) % Size);
			return f.Construct();
		}

		public bool Equals(IHashingFunctionScheme? other)
		{
			if (other is LinearCongruenceScheme lcs)
			{
				return lcs.Multiply == Multiply && lcs.Add == Add && lcs.Size == Size;
			}
			return false;
		}
	}
}