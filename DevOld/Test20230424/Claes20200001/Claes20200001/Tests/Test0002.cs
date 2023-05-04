using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Charlotte.Commons;

namespace Charlotte.Tests
{
	public class Test0002
	{
		public void Test01()
		{
			ProcMain.WriteLog("*1");
			SCommon.Batch(new string[] { "TIMEOUT 3" }, "", SCommon.StartProcessWindowStyle_e.MINIMIZED);
			ProcMain.WriteLog("*2");
		}

		public void Test02()
		{
			for (int c = 0; c < 30; c++)
			{
				Console.WriteLine(SCommon.CRandom.GetBoolean());
			}
			for (int c = 0; c < 30; c++)
			{
				Console.WriteLine(SCommon.CRandom.GetSign());
			}
		}

		public void Test03()
		{
			RandomUnit ru = new RandomUnit(new Test03_RNG_01());

			for (int c = 0; c < 100; c++)
			{
				if (!ru.GetBoolean())
					throw null;

				if (ru.GetBoolean())
					throw null;
			}
			for (int c = 0; c < 100; c++)
			{
				if (ru.GetSign() != 1)
					throw null;

				if (ru.GetSign() != -1)
					throw null;
			}
			Console.WriteLine("OK");

			// ----

			bool[] table = Enumerable.Range(0, 10000).Select(dummy => SCommon.CRandom.GetBoolean()).ToArray();

			ru = new RandomUnit(new Test03_RNG_02(table));

			for (int loop = 0; loop < 100; loop++)
			{
				for (int c = 0; c < table.Length; c++)
					if (ru.GetBoolean() != table[c])
						throw null;

				for (int c = 0; c < table.Length; c++)
					if (ru.GetSign() != (table[c] ? 1 : -1))
						throw null;
			}
			Console.WriteLine("OK");

			// ----

			Console.WriteLine("OK!");
		}

		private class Test03_RNG_01 : RandomUnit.IRandomNumberGenerator
		{
			public byte[] GetBlock()
			{
				return new byte[] { 0x55 };
			}

			public void Dispose()
			{
				// noop
			}
		}

		private class Test03_RNG_02 : RandomUnit.IRandomNumberGenerator
		{
			private Func<byte[]> Generator;

			public Test03_RNG_02(bool[] table)
			{
				this.Generator =
					SCommon.Supplier(this.E_BytesGenerator(
						SCommon.Supplier(this.E_ByteGenerator(
							SCommon.Supplier(this.E_Endless(
								table
								))
							))
						, () => SCommon.CRandom.GetRange(1, 100)
						));
			}

			private IEnumerable<byte[]> E_BytesGenerator(Func<byte> byteGenerator, Func<int> sizeGenerator)
			{
				for (; ; )
				{
					int size = sizeGenerator();
					byte[] data = new byte[size];

					for (int c = 0; c < size; c++)
						data[c] = byteGenerator();

					yield return data;
				}
			}

			private IEnumerable<byte> E_ByteGenerator(Func<bool> bitGenerator)
			{
				for (; ; )
				{
					int value = 0;

					for (int c = 0; c < 8; c++)
						if (bitGenerator())
							value |= 1 << c;

					yield return (byte)value;
				}
			}

			private IEnumerable<T> E_Endless<T>(T[] list)
			{
				for (; ; )
					foreach (T element in list)
						yield return element;
			}

			public byte[] GetBlock()
			{
				return this.Generator();
			}

			public void Dispose()
			{
				// noop
			}
		}
	}
}
