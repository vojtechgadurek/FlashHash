using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LashHash.SchemesAndFamilies
{
	public interface IHashingFunctionScheme
	{
		public Expression<HashingFunction> Create();
	}
}
