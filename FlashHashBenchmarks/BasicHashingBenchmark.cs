﻿using BenchmarkDotNet.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlashHashBenchmarks
{
    public class HashingBenchmark
    {

        [ParamsSource(nameof(HashingFunctionsToTest))]
        public Type hashingFunctionFamily;
        public static IEnumerable<Type> HashingFunctionsToTest()
        {
            return HashFunctionProvider.GetAllHashingFunctionFamilies();
        }

        public const int Length = 4 * 1024;
        public const int BufferLength = 1096;
        public const int DataLength = Length * BufferLength;
        public Random Random = new Random();
        public RandomData Stream;

        Action<ulong[], ulong[], int, int> f;



        [GlobalSetup]
        public void GlobalSetup()
        {
            Stream = new RandomData((int)DataLength, Random);
        }

        [IterationSetup]
        public void IterationSetup()
        {
            Stream.Reset();
            f = LittleSharp.Utils.Buffering.BufferFunction(
                HashFunctionProvider.Get(hashingFunctionFamily, 1000, 0).Create()).Compile();
        }
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
