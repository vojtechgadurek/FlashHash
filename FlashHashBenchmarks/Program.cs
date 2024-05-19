using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BenchmarkDotNet.Running;
using FlashHashBenchmarks;

namespace FlashHashBenchmarks
{
	public class Program
	{
		public static void Main(string[] args)
		{
			if (args[0] == "Explicit-Delegate-ExpressionTrees")
			{
				var summary = BenchmarkRunner.Run<ExplicitDelegateExpressionTreesHashingFunctionBenchmark>();
			}
		}
	}
}
