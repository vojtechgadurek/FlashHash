using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlashHash.SchemesAndFamilies
{
    public interface IHashFunctionScheme : IEquatable<IHashFunctionScheme>
    {
        public Expression<HashingFunction> Create();
        public ulong Size { get; }
    }
}
