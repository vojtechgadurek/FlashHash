using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LashHashBenchmarks.Utils
{
	public class RandomData
	{
		ulong[] data;
		int counter = 0;
		public RandomData(int size, Random random)
		{
			data = new ulong[size];
			FillWithNewRandomData(random);
		}

		public void FillWithNewRandomData(Random random)
		{
			for (int i = 0; i < data.Length; i++)
			{
				data[i] = (ulong)random.NextInt64();
			}
			counter = 0;
		}


		public void Reset()
		{
			counter = 0;
		}

		public int FillBuffer(ulong[] buffer)
		{
			int numberOfItemsReturned = buffer.Length;
			if (counter + buffer.Length > data.Length)
			{
				numberOfItemsReturned = data.Length - (int)counter;
			}

			Array.Copy(data, counter, buffer, 0, numberOfItemsReturned);

			counter += numberOfItemsReturned;
			return numberOfItemsReturned;
		}
	}
}
