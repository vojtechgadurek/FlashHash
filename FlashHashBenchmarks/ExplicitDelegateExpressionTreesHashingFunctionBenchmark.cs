using BenchmarkDotNet.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlashHashBenchmarks
{
	public class ExplicitDelegateExpressionTreesHashingFunctionBenchmark
	{
		public const int Length = 1024;
		public const int BufferLength = 4096;
		public const int DataLength = Length * BufferLength;
		public Random Random;
		public RandomData Stream;

		[GlobalSetup]
		public void GlobalSetup()
		{
			Random = new Random();
			Stream = new RandomData((int)DataLength, Random);
		}

		[IterationSetup]
		public void IterationSetup()
		{
			Stream.Reset();
		}

		[Benchmark]
		public ulong LinearCongruenceExpressionTrees()
		{
			ulong sum = 0;
			ulong[] buffer = new ulong[BufferLength];
			ulong[] answerBuffer = new ulong[BufferLength];
			Action<ulong[], ulong[], int, int> f = LittleSharp.Utils.Buffering.BufferFunction(
				HashingFunctionProvider.Get(typeof(LinearCongruenceFamily), (ulong)Random.NextInt64(0, 2 << 60), 0).Create()).Compile(); ;

			while (Stream!.FillBuffer(buffer) > 0)
			{
				f(buffer, answerBuffer, 0, BufferLength);

				for (int j = 0; j < BufferLength; j++)
				{
					sum += answerBuffer[j];
				}
			}
			return sum;
		}


		[Benchmark(Baseline = true)]
		public ulong LinearCongruenceDelegate()
		{
			ulong a = (ulong)Random!.NextInt64();
			ulong b = (ulong)Random.NextInt64();
			ulong size = (ulong)Random.NextInt64(0, 2 << 60);
			ulong prime = LinearCongruenceScheme.GetGoodPrime(size);
			Action<ulong[], ulong[], int, int> f = (input, output, start, length) =>
			{
				for (int i = 0; i < length; i++)
				{
					output[start + i] = (input[start + i] * a + b) % prime % size;
				}
			};


			ulong sum = 0;
			ulong[] buffer = new ulong[BufferLength];
			ulong[] answerBuffer = new ulong[BufferLength];

			while (Stream!.FillBuffer(buffer) > 0)
			{
				f(buffer, answerBuffer, 0, BufferLength);

				for (int j = 0; j < BufferLength; j++)
				{
					sum += answerBuffer[j];
				}
			}
			return sum;
		}


		[Benchmark]

		public ulong LinearCongruenceExplicit()
		{
			ulong a = (ulong)Random!.NextInt64();
			ulong b = (ulong)Random.NextInt64();
			ulong size = (ulong)Random.NextInt64(0, 2 << 60);
			ulong prime = LinearCongruenceScheme.GetGoodPrime(size);
			Action<ulong[], ulong[], int, int> f = (input, output, start, length) =>
			{
				for (int i = 0; i < length; i++)
				{
					output[start + i] = (input[start + i] * 122032421 + 129934) % 18338 % 3400;
				}
			};


			ulong sum = 0;
			ulong[] buffer = new ulong[BufferLength];
			ulong[] answerBuffer = new ulong[BufferLength];

			while (Stream!.FillBuffer(buffer) > 0)
			{
				f(buffer, answerBuffer, 0, BufferLength);

				for (int j = 0; j < BufferLength; j++)
				{
					sum += answerBuffer[j];
				}
			}
			return sum;
		}
	}
}
