using LashHash.SchemesAndFamilies;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LashHash.SchemesAndFamilies
{
	public interface IHashingFunctionFamily
	{
		public IHashingFunctionScheme GetScheme(ulong size);

		public void SetRandomness(Random random);
	}
	public interface IHashingFunctionFamily<THashingFunctionScheme> : IHashingFunctionFamily where THashingFunctionScheme : IHashingFunctionScheme
	{
		new public THashingFunctionScheme GetScheme(ulong size);

	}
}
