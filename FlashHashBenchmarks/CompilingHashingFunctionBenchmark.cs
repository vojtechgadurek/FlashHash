using BenchmarkDotNet.Attributes;
using LashHash;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LashHashBenchmarks
{
	public class CompilingHashingFunctionBenchmark
	{
		[ParamsSource(nameof(HashingFunctionsToTest))]
		public Type hashingFunctionFamily;
		public static IEnumerable<Type> HashingFunctionsToTest()
		{
			return HashingFunctionProvider.GetAllHashingFunctionFamilies();
		}

		[Benchmark]
		public Delegate CompileHashFunction()
		{
			return LittleSharp.Utils.Buffers.BufferFunction(
				HashingFunctionProvider.Get(hashingFunctionFamily, 4096).Create()).Compile();
		}
	}
}
