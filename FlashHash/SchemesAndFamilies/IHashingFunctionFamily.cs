﻿using FlashHash.SchemesAndFamilies;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlashHash.SchemesAndFamilies
{
	public interface IHashingFunctionFamily
	{
		public IHashingFunctionScheme GetScheme(ulong size, ulong offset);

		public void SetRandomness(Random random);
	}
	public interface IHashingFunctionFamily<THashingFunctionScheme> : IHashingFunctionFamily where THashingFunctionScheme : IHashingFunctionScheme
	{
		new public THashingFunctionScheme GetScheme(ulong size, ulong offset);

	}
}
