using Gee.External.Capstone.X86;
using FlashHash.SchemesAndFamilies;
using LittleSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Dynamic;
using System.Linq;
using System.Linq.Expressions;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace FlashHash.Utils
{
    public class HashingFunctionCache<THashingFunctionScheme> where THashingFunctionScheme : IHashFunctionScheme
    {
        private readonly IDictionary<THashingFunctionScheme, Expression<HashingFunction>> _cache
            = new Dictionary<THashingFunctionScheme, Expression<HashingFunction>>();

        public Expression<HashingFunction> Get(THashingFunctionScheme scheme, bool cacheResult = true)
        {
            if (!_cache.TryGetValue(scheme, out var value))
            {
                value = scheme.Create();
                if (cacheResult) _cache[scheme] = value;
            }
            return value;
        }
    }
}
