using FlashHash.SchemesAndFamilies;
using Microsoft.CodeAnalysis.Emit;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;


namespace FlashHash
{

	public static class HashingFunctionProvider
	{
		static IDictionary<Type, IHashingFunctionFamily> hashingFunctions = new Dictionary<Type, IHashingFunctionFamily>();

		static HashingFunctionProvider()
		{
			var type = typeof(IHashingFunctionFamily);
			var types = AppDomain.CurrentDomain.GetAssemblies()
				.SelectMany(s => s.GetTypes())
				.Where(p => type.IsAssignableFrom(p) && !p.IsInterface && !p.IsAbstract);
			foreach (var t in types)
			{
				var instance = (IHashingFunctionFamily)Activator.CreateInstance(t)!;
				instance.SetRandomness(new Random());
				hashingFunctions.Add(t, instance);
			}
		}

		public static IEnumerable<Type> GetAllHashingFunctionFamilies() => hashingFunctions.Keys;
		public static IHashingFunctionScheme Get<THashingFunctionFamily>(ulong size) where THashingFunctionFamily : IHashingFunctionFamily
		{
			var family = hashingFunctions[typeof(THashingFunctionFamily)];
			return family.GetScheme(size);
		}
		public static IHashingFunctionScheme Get(Type hashingFunctionFamily, ulong size)
		{
			var family = hashingFunctions[hashingFunctionFamily];
			return family.GetScheme(size);
		}

	}
}
