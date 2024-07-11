using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace FlashHash.SchemesAndFamilies
{
	public record class PolynomialFamily : IHashingFunctionFamily<PolynomialScheme>
	{
		Random? _random = new Random();
		int _polynomialOrder = 2;

		public PolynomialFamily()
		{
		}
		public PolynomialFamily(int polynomialOrder)
		{
			_polynomialOrder = polynomialOrder;
		}
		ulong[] GetCoefficients(int polynomialOrder)
		{
			ulong[] c = new ulong[polynomialOrder + 1];
			for (int i = 0; i < c.Length; i++) c[i] =
					(ulong)(RandomNumberGenerator.GetInt32(int.MinValue, int.MaxValue) << 32)
					+ ((ulong)RandomNumberGenerator.GetInt32(int.MinValue, int.MaxValue));
			return c;
		}
		public PolynomialScheme GetScheme(ulong size, ulong offset)
		{
			return new PolynomialScheme(GetCoefficients(_polynomialOrder), size, offset);
		}

		public void SetRandomness(Random random)
		{
			_random = random;
		}

		IHashingFunctionScheme IHashingFunctionFamily.GetScheme(ulong size, ulong offset)
		{
			return GetScheme(size, offset);
		}
	}

	public record struct PolynomialScheme(ulong[] Coefficients, ulong Size, ulong Offset) : IHashingFunctionScheme
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

		static PolynomialScheme()
		{
			foreach (var exponent in MersennePrimesExponents)
			{
				MersennePrimes.Add((2UL << exponent) - 1UL);
			}
		}
		public ulong GetGoodPrime(ulong size)
		{
			//https://mj.ucw.cz/vyuka/dsnotes/06-hash.pdf page 7 collorary 
			ulong requiredPrime = size * 2 * ((ulong)Coefficients.Length - 1);
			var index = MersennePrimes.FindIndex(x => x > size);
			if (index == -1)
			{
				throw new InvalidOperationException("Size is larger than 2^61");
			}
			return MersennePrimes[index];
		}

		public Expression<HashingFunction> Create()
		{
			var f = new CompiledFunction<ulong, ulong>(out var value_);

			f.S.Assign(f.Output, 0);

			for (int i = 0; i < Coefficients.Length; i++)
			{
				f.S.Assign(f.Output, f.Output.V * value_.V + Coefficients[i]);
			}

			f.S.Assign(f.Output, (f.Output.V % GetGoodPrime(Size) % Size) + Offset);
			return f.Construct();
		}
		public bool Equals(IHashingFunctionScheme? other)
		{
			if (other is PolynomialScheme lcs)
			{
				return lcs.Coefficients.SequenceEqual(Coefficients) && lcs.Size == Size && lcs.Offset == Offset;
			}
			return false;
		}
	}
}
