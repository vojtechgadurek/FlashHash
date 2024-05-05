using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlashHash.SchemesAndFamilies
{
	public interface IHashingFunctionScheme : IEquatable<IHashingFunctionScheme>
	{
		public Expression<HashingFunction> Create();
	}
}
