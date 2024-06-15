using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlashHash.SchemesAndFamilies
{
	public class TabulationFamily : IHashingFunctionFamily<TabulationScheme>
	{
		static Random _random = new Random();
		public TabulationScheme GetScheme(ulong size, ulong offset)
		{
			return new TabulationScheme(GetTable(), size, offset);
		}

		public static ulong[][] GetTable()
		{
			ulong[][] table = new ulong[8][];
			for (int i = 0; i < 8; i++)
			{
				table[i] = new ulong[256];

				var t = table[i];
				for (int j = 0; j < 256; j++)
				{
					t[j] = (ulong)_random.NextInt64();
				}
			}
			return table;

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

	public record struct TabulationScheme(ulong[][] Table, ulong Size, ulong Offset) : IHashingFunctionScheme
	{
		public Expression<HashingFunction> Create()
		{
			const int length = 8;
			var f = CompiledFunctions.Create<ulong, ulong>(out var input_);
			f.S.DeclareVariable<int>(out var i_, 0)
				.DeclareVariable<ulong[][]>(out var table_, Table)
				.While(i_.V < length,
					new Scope().DeclareVariable<int>(out var index_, input_.V.Convert<int>() % (1 << length))
					.Assign(f.Output, f.Output.V ^ table_.V.ToTable<ulong[]>()[i_.V].V.ToTable<ulong>()[index_.V].V))
				);
			return f.Construct();
		}

		public bool Equals(IHashingFunctionScheme? other)
		{
			throw new NotImplementedException();
		}
	}
}
