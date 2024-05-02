using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using FlashHash.Utils;

namespace FlashHash.SchemesAndFamilies
{
	public class MultiplyShiftFamily : IHashingFunctionFamily<MultiplyShift>
	{
		private Random _random;
		public MultiplyShift GetScheme(ulong size)
		{
			return new MultiplyShift((ulong)_random.Next(), size);
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
	public record struct MultiplyShift(ulong Multiply, ulong Size) : IHashingFunctionScheme
	{
		public Expression<HashingFunction> Create()
		{
			var kMerLength = BitOperations.LeadingZeroCount(Size);
			var f = new CompiledFunction<ulong, ulong>(out var value_);
			f.S.Assign(f.Output, (value_.V * Multiply >> 64 - kMerLength) % Size);
			return f.Construct();
		}
	}

}
