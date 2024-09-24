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

    public static class HashFunctionProvider
    {
        static IDictionary<Type, IHashFunctionFamily> hashFunctions = new Dictionary<Type, IHashFunctionFamily>();
        //static IDictionary<Type, Type> hashFamiliesToScheme = new Dictionary<Type, Type>();


        static HashFunctionProvider()
        {
            var type = typeof(IHashFunctionFamily);
            var types = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(s => s.GetTypes())
                .Where(p => type.IsAssignableFrom(p) && !p.IsInterface && !p.IsAbstract);
            foreach (var t in types)
            {
                var instance = (IHashFunctionFamily)Activator.CreateInstance(t)!;
                instance.SetRandomness(new Random());
                hashFunctions.Add(t, instance);

                //hashFamiliesToScheme.Add(t, t /*.GetInterfaces().Where(x => x.Name == "IHashFunctionFamily`1").First()*/.GenericTypeArguments[0]);
            }
        }

        public static Type GetFamilyByName(string name) => hashFunctions.Keys.First(x => x.Name == name);

        public static IEnumerable<Type> GetAllHashingFunctionFamilies() => hashFunctions.Keys;
        public static IHashFunctionScheme Get<THashingFunctionFamily>(ulong size, ulong offset) where THashingFunctionFamily : IHashFunctionFamily
        {
            var family = hashFunctions[typeof(THashingFunctionFamily)];
            return family.GetScheme(size, offset);
        }
        public static IHashFunctionScheme Get(Type hashingFunctionFamily, ulong size, ulong offset)
        {
            var family = hashFunctions[hashingFunctionFamily];
            return family.GetScheme(size, offset);
        }

    }
}
