using System.Numerics;
using Xunit.Abstractions;

namespace FlashHashTests
{
	namespace Tests
	{
		public class ModuloTest
		{
			[Fact]
			public void TestModulo()
			{
				Random random = new Random(42);
				for (ulong i = 0; i < 100; i++)
				{
					ulong randomSize = (ulong)random.NextInt64();
					var modulo = new ModuloScheme(randomSize, 0).Create().Compile();

					ulong randomValue = (ulong)random.NextInt64();
					Assert.Equal(modulo(randomValue), randomValue % randomSize);
				}
			}

		}

		public class MultiplyShiftTest
		{

		}

		public class TabulationHashing
		{
			[Fact]
			void BasicTest()
			{
				ulong[][] table = new ulong[8][];
				for (ulong i = 0; i < 8; i++)
				{
					table[i] = new ulong[256];

					for (ulong j = 0; j < 256; j++)
					{
						table[i][j] = j << (int)(i * 8);
					}
				}

				var m = Map.GetMap(Enumerable.Range(0, 256).Select(x => (ulong)x).Zip(table[0])).Compile();

				for (ulong i = 0; i < 256; i++) Assert.Equal(i, m(i));
				const ulong size = 10;

				var tab = new TabulationScheme(table, size, 0);
				var hf = tab.Create().Compile();


				for (ulong i = 0; i < 256; i++) Assert.Equal(i % size, hf(i));


				for (ulong i = 0; i < 1000; i++)
				{
					var ran = (ulong)Random.Shared.NextInt64();
					Assert.Equal(ran % size, hf(ran));
				}

			}


			public class HashingFunctionProviderTest
			{
				//output is a list of all hashing function families

				private readonly ITestOutputHelper _output;
				public HashingFunctionProviderTest(ITestOutputHelper output)
				{
					_output = output;
				}
				[Fact]
				public void TestGetAllHashingFunctionFamilies()
				{
					var families = HashingFunctionProvider.GetAllHashingFunctionFamilies();
					foreach (var family in families)
					{
						_output.WriteLine(family.ToString());
					}
					Assert.NotEmpty(families);
				}

				[Fact]
				public void TestGet()
				{
					var families = HashingFunctionProvider.GetAllHashingFunctionFamilies();
					foreach (var family in families)
					{
						var scheme = HashingFunctionProvider.Get(family, 42, 0);

						_output.WriteLine(scheme.ToString());
						Assert.NotNull(scheme);
					}
				}
			}

		}
	}
}