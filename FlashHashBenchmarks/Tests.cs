using BenchmarkDotNet.Attributes;
using LashHash;



public class HashingBenchmark
{

	[ParamsSource(nameof(HashingFunctionsToTest))]
	public Type hashingFunctionFamily;
	public static IEnumerable<Type> HashingFunctionsToTest()
	{
		return HashingFunctionProvider.GetAllHashingFunctionFamilies();
	}

	public const int Length = 1024;
	public const int BufferLength = 4096;
	public const int DataLength = Length * BufferLength;
	public Random? Random;
	public RandomData Stream;

	Action<ulong[], ulong[], int, int> f;



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
		f = LittleSharp.Utils.Buffers.BufferFunction(
			HashingFunctionProvider.Get(hashingFunctionFamily, 4096).Create()).Compile();
	}
	[Benchmark]
	public ulong[] BenchmarkHashingFunctionParallel()
	{

		ulong sum = 0;
		ulong[] buffer = new ulong[BufferLength];
		ulong[] answerBuffer = new ulong[BufferLength];

		int numberOfDivisions = 8;
		int divisionSize = BufferLength / numberOfDivisions;

		ulong[] sums = new ulong[numberOfDivisions];

		while (Stream!.FillBuffer(buffer) > 0)
		{
			Parallel.For(0, numberOfDivisions, (j) =>
			{
				int offset = j * divisionSize;
				f(buffer, answerBuffer, offset, divisionSize);
			});
		}
		return answerBuffer;
	}

	[Benchmark]
	public ulong BenchmarkHashingFunction()
	{
		Action<ulong[], ulong[], int, int> f = LittleSharp.Utils.Buffers.BufferFunction(
			HashingFunctionProvider.Get(hashingFunctionFamily, 4096).Create()).Compile();
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

