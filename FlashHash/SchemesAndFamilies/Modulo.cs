using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FlashHash.Utils;

namespace FlashHash.SchemesAndFamilies
{
	public class ModuloFamily : IHashingFunctionFamily<ModuloScheme>
	{
		Random? _random;
		public void SetRandomness(Random random)
		{
			_random = random;
		}
		public ModuloScheme GetScheme(ulong size)
		{
			return new ModuloScheme(size);
		}

		IHashingFunctionScheme IHashingFunctionFamily.GetScheme(ulong size)
		{
			return GetScheme(size);
		}

	}
	public record struct ModuloScheme(ulong Size) : IHashingFunctionScheme
	{
		public Expression<HashingFunction> Create()
		{
			var f = new CompiledFunction<ulong, ulong>(out var value_);
			f.S.Assign(f.Output, value_.V % Size);
			return f.Construct();
		}

		public bool Equals(IHashingFunctionScheme? other)
		{
			if (other is ModuloScheme modulo)
			{
				return modulo.Size == Size;
			}
			else
			{
				return false;
			}
		}
	}
}
