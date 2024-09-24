using FlashHash.SchemesAndFamilies;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlashHash.SchemesAndFamilies
{
    public interface IHashFunctionFamily
    {
        public IHashFunctionScheme GetScheme(ulong size, ulong offset);

        public void SetRandomness(Random random);
    }
    public interface IHashFunctionFamily<THashingFunctionScheme> : IHashFunctionFamily where THashingFunctionScheme : IHashFunctionScheme
    {
        new public THashingFunctionScheme GetScheme(ulong size, ulong offset);

    }
}
