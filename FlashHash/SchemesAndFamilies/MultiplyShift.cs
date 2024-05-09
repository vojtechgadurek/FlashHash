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
		private Random _random = new Random();
		public MultiplyShift GetScheme(ulong size, ulong offset)
		{
			return new MultiplyShift((ulong)_random.Next(), size, offset);
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
	public record struct MultiplyShift(ulong Multiply, ulong Size, ulong Offset) : IHashingFunctionScheme
	{
		public Expression<HashingFunction> Create()
		{
			var kMerLength = BitOperations.LeadingZeroCount(Size);
			var f = new CompiledFunction<ulong, ulong>(out var value_);
			f.S.Assign(f.Output, (value_.V * Multiply >> 64 - kMerLength) % Size + Offset);
			return f.Construct();
		}

		public bool Equals(IHashingFunctionScheme? other)
		{
			if (other is MultiplyShift ms)
			{
				return ms.Size == Size && ms.Multiply == Multiply && ms.Offset == Offset;
			}
			else
			{
				return false;
			}
		}
	}

}
