using LashHash.SchemesAndFamilies;
using System.Numerics;
using Xunit.Abstractions;

namespace LashHashTests
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
					var modulo = new ModuloScheme(randomSize).Get().Compile();

					ulong randomValue = (ulong)random.NextInt64();
					Assert.Equal(modulo(randomValue), randomValue % randomSize);
				}
			}

		}

		public class MultiplyShiftTest
		{

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
					var scheme = HashingFunctionProvider.Get(family, 42);

					_output.WriteLine(scheme.ToString());
					Assert.NotNull(scheme);
				}
			}
		}

	}
}