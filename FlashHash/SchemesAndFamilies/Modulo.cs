using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FlashHash.Utils;

namespace FlashHash.SchemesAndFamilies
{
    public class ModuloFamily : IHashFunctionFamily<ModuloScheme>
    {
        Random? _random;
        public void SetRandomness(Random random)
        {
            _random = random;
        }
        public ModuloScheme GetScheme(ulong size, ulong offset)
        {
            return new ModuloScheme(size, offset);
        }

        IHashFunctionScheme IHashFunctionFamily.GetScheme(ulong size, ulong offset)
        {
            return GetScheme(size, offset);
        }

    }
    public readonly record struct ModuloScheme(ulong Size, ulong Offset) : IHashFunctionScheme
    {
        public Expression<HashingFunction> Create()
        {
            var f = new CompiledFunction<ulong, ulong>(out var value_);
            f.S.Assign(f.Output, value_.V % Size + Offset);
            return f.Construct();
        }

        public bool Equals(IHashFunctionScheme? other)
        {
            if (other is ModuloScheme modulo)
            {
                return modulo.Size == Size && modulo.Offset == Offset;
            }
            else
            {
                return false;
            }
        }
    }
}
