using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using Charlotte.Commons;
using System.IO;
using System.IO.Compression;

namespace Charlotte.Tests
{
	public class Test0002
	{
		public void Test01()
		{
			int[,] t1 = new int[100, 100]; // OK
			int[,] t2 = new int[0, 100]; // OK
			int[,] t3 = new int[100, 0]; // OK
			int[,] t4 = new int[0, 0]; // OK

			Console.WriteLine(t1);
			Console.WriteLine(t2);
			Console.WriteLine(t3);
			Console.WriteLine(t4);

			// ----

			Bitmap bmp1 = new Bitmap(100, 100); // OK
			//Bitmap bmp2 = new Bitmap(0, 100); // 例外
			//Bitmap bmp3 = new Bitmap(100, 0); // 例外
			//Bitmap bmp4 = new Bitmap(0, 0); // 例外

			Console.WriteLine(bmp1);
			//Console.WriteLine(bmp2);
			//Console.WriteLine(bmp3);
			//Console.WriteLine(bmp4);

			// ----

			int[,] u1 = new int[2100000000, 0]; // OK
			int[,] u2 = new int[0, 2100000000]; // OK
			int[,] u3 = new int[10000, 10000]; // OK
			//int[,] u4 = new int[100000, 100000]; // メモリ不足
			//int[,] u5 = new int[1000000, 1000000]; // メモリ不足

			Console.WriteLine(u1);
			Console.WriteLine(u2);
			Console.WriteLine(u3);
			//Console.WriteLine(u4);
			//Console.WriteLine(u5);
		}

		public void Test02()
		{
			Test02_a(45);
			Test02_a(46);
			Test02_a(47);
			Test02_a(48); // <--- RandomUnit.GetRate
			Test02_a(49);
			Test02_a(50);
			Test02_a(51);
			Test02_a(52); // ここが限界っぽい。== doubleの仮数部のビット数
			Test02_a(53);
			Test02_a(54);
			Test02_a(55);
		}

		private void Test02_a(int bits)
		{
			Console.WriteLine("bits: " + bits);

			for (int n = 0; n <= 5; n++)
			{
				double value = 1.0;

				value += (double)n / ((1UL << bits) - 1);

				//Console.WriteLine(value.ToString("F20")); // 十分な桁数があるけど正確な値が出ないっぽい。
				Console.WriteLine(value.ToString("R"));
			}
		}

		public void Test03()
		{
			Console.WriteLine("Range:");
			foreach (int value in Enumerable.Range(0, 5))
			{
				Console.WriteLine(value);
			}

			Console.WriteLine("Repeat:");
			foreach (int value in Enumerable.Repeat(0, 5))
			{
				Console.WriteLine(value);
			}
		}

		public void Test04()
		{
			Func<string> generator = () =>
			{
				string value = Guid.NewGuid().ToString("B");
				Console.WriteLine("生成した値：" + value);
				return value;
			};

			foreach (string value in SCommon.Generate(5, generator))
			{
				Console.WriteLine("取得した値：" + value);
			}

			// ----

			Console.WriteLine(SCommon.Generate(10, () => 1).Count());
			Console.WriteLine(SCommon.Generate(20, () => 1).Count());
			Console.WriteLine(SCommon.Generate(30, () => 1).Count());
			Console.WriteLine(SCommon.Generate(40, () => 1).Count());
			Console.WriteLine(SCommon.Generate(50, () => 1).Count());

			// ----

			Console.WriteLine(SCommon.Generate(0, () => 1).Count());
			Console.WriteLine(SCommon.Generate(-1, () => 1).Take(1000).Count());
			Console.WriteLine(SCommon.Generate(-1, () => 1).Take(2345).Count());
		}

		public void Test05()
		{
			string str = SCommon.HALF;

			for (int c = 0; c < 20; c++)
			{
				str = SCommon.Serializer.I.Join(new string[] { str });

				Console.WriteLine(str.Length + " : " + str);
			}
		}

		public void Test06()
		{
			SCommon.Batch(new string[] { @"DIR > C:\temp\1.txt" }, @"C:\temp");

			FileStream reader;
			FileStream writer;

			using (reader = new FileStream(@"C:\temp\1.txt", FileMode.Open, FileAccess.Read))
			using (writer = new FileStream(@"C:\temp\2.txt", FileMode.Create, FileAccess.Write))
			{
				using (GZipStream gz = new GZipStream(writer, CompressionMode.Compress)) // gzを閉じたらwriterも閉じる。
				{
					reader.CopyTo(gz);

					Console.WriteLine(reader.CanRead); // True
					Console.WriteLine(writer.CanWrite); // True
				}
				Console.WriteLine(reader.CanRead); // True
				Console.WriteLine(writer.CanWrite); // False
			}
			Console.WriteLine(reader.CanRead); // False
			Console.WriteLine(writer.CanWrite); // False

			using (reader = new FileStream(@"C:\temp\1.txt", FileMode.Open, FileAccess.Read))
			using (writer = new FileStream(@"C:\temp\2.txt", FileMode.Create, FileAccess.Write))
			{
				using (GZipStream gz = new GZipStream(writer, CompressionMode.Compress, true)) // gzを閉じてもwriterは閉じない。
				{
					reader.CopyTo(gz);

					Console.WriteLine(reader.CanRead); // True
					Console.WriteLine(writer.CanWrite); // True
				}
				Console.WriteLine(reader.CanRead); // True
				Console.WriteLine(writer.CanWrite); // True
			}
			Console.WriteLine(reader.CanRead); // False
			Console.WriteLine(writer.CanWrite); // False

			using (reader = new FileStream(@"C:\temp\2.txt", FileMode.Open, FileAccess.Read))
			using (writer = new FileStream(@"C:\temp\1.txt", FileMode.Create, FileAccess.Write))
			{
				using (GZipStream gz = new GZipStream(reader, CompressionMode.Decompress)) // gzを閉じたらreaderも閉じる。
				{
					gz.CopyTo(writer);

					Console.WriteLine(reader.CanRead); // True
					Console.WriteLine(writer.CanWrite); // True
				}
				Console.WriteLine(reader.CanRead); // False
				Console.WriteLine(writer.CanWrite); // True
			}
			Console.WriteLine(reader.CanRead); // False
			Console.WriteLine(writer.CanWrite); // False

			using (reader = new FileStream(@"C:\temp\2.txt", FileMode.Open, FileAccess.Read))
			using (writer = new FileStream(@"C:\temp\1.txt", FileMode.Create, FileAccess.Write))
			{
				using (GZipStream gz = new GZipStream(reader, CompressionMode.Decompress, true)) // gzを閉じてもreaderは閉じない。
				{
					gz.CopyTo(writer);

					Console.WriteLine(reader.CanRead); // True
					Console.WriteLine(writer.CanWrite); // True
				}
				Console.WriteLine(reader.CanRead); // True
				Console.WriteLine(writer.CanWrite); // True
			}
			Console.WriteLine(reader.CanRead); // False
			Console.WriteLine(writer.CanWrite); // False
		}

		public void Test07()
		{
			byte[] data = Encoding.UTF8.GetBytes("Mercury, Venus, Earth, Mars, Jupiter, Saturn, Uranus, Neptune, Planet-9");
			//byte[] data = SCommon.CRandom.GetBytes(30);
			//byte[] data = SCommon.CRandom.GetBytes(100);
			byte[] gzData = SCommon.Compress(data);
			string gzStr = SCommon.Base64.I.Encode(gzData);

			Console.WriteLine(SCommon.Hex.I.ToString(data));
			Console.WriteLine(SCommon.Hex.I.ToString(gzData));
			Console.WriteLine(gzStr);

			// ----

			string zs = "H4sIAAAAAAAA//NNLUouLarUUQhLzSst1lFwTSwqydBR8E0sAnK8SgsyS1KLdBSCE0tKi/J0FEKLEsGq/FILSkrzUnUUAnIS81JLdC0Bufv0hEcAAAA="; // Java
			byte[] zd = SCommon.Base64.I.Decode(zs);
			byte[] dd = SCommon.Decompress(zd);
			string ds = Encoding.UTF8.GetString(dd);

			Console.WriteLine(ds);
		}

		public void Test08()
		{
			for (int testcnt = 0; testcnt < 10000; testcnt++)
			{
				byte[] data = SCommon.CRandom.GetBytes(SCommon.CRandom.GetInt(1000));

				//Console.WriteLine(data.Length); // cout

				using (MemoryStream mem = new MemoryStream())
				{
					int offset = 0;

					while (offset < data.Length)
					{
						int size = SCommon.CRandom.GetInt(1300);
						size = Math.Min(size, data.Length - offset);

						mem.Write(SCommon.GetPart(data, offset, size), 0, size);

						offset += size;
					}

					// メモリストリームは閉じても有効である想定
					//
					mem.Close();
					mem.Dispose();

					// ----

					byte[] retData = mem.ToArray();

					if (retData == null)
						throw null;

					//Console.WriteLine(retData.Length); // cout

					if (SCommon.Comp(data, retData) != 0) // ? 不一致
						throw null;
				}
			}
		}
	}
}
