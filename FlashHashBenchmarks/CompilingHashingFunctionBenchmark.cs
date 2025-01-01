using BenchmarkDotNet.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlashHashBenchmarks
{
    public class CompilingHashingFunctionBenchmark
    {
        [ParamsSource(nameof(HashingFunctionsToTest))]
        public Type hashingFunctionFamily;
        public static IEnumerable<Type> HashingFunctionsToTest()
        {
            return HashFunctionProvider.GetAllHashingFunctionFamilies();
        }

        [Benchmark]
        public Delegate CompileHashFunction()
        {
            return LittleSharp.Utils.Buffering.BufferFunction(
                HashFunctionProvider.Get(hashingFunctionFamily, 4096, 0).Create()).Compile();
        }
    }
}
