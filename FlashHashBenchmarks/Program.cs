using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BenchmarkDotNet.Running;
using FlashHashBenchmarks;

namespace LashHashBenchmarks
{
	public class Program
	{
		public static void Main(string[] args)
		{
			var summary = BenchmarkRunner.Run<CompareHashingFunctionsCompiledNotCompiled>();
		}
	}
}
