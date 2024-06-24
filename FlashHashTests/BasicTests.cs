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
				}

				table[0][1] = 1;

				var tabulation = new TabulationScheme(table, 10, 0).Create().Compile();

				Assert.Equal(1UL, tabulation(1));

				table[0][1] = 3;

				Assert.Equal(3UL, tabulation(1));

				table[0][1] = 0;


				table[1][1] = 4;

				Assert.Equal(4UL, tabulation(0b1_0000_0000));


				table[0][2] = 4;

				Assert.Equal(0UL, tabulation(0b1_0000_0010));

				var fam = new TabulationFamily();

				var h1 = fam.GetScheme(10, 0);
				var h2 = fam.GetScheme(10, 0);

				var x = 0;









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