using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Drawing;
using System.Windows.Forms;
using Charlotte.Commons;

namespace Charlotte.Tests
{
	public class Test0001
	{
		public void Test01()
		{
			File.WriteAllBytes(SCommon.NextOutputPath() + ".txt", SCommon.GetJCharBytes().ToArray());
		}

		public void Test02()
		{
			for (int c = 0; c < 50; c++)
			{
				Console.WriteLine(SCommon.CRandom.GetBoolean());
			}
		}

		public void Test03()
		{
			Test03_a("ABCDEF", 1, 4, "BCDE");
			Test03_a("ABCDEF", 0, 3, "ABC");
			Test03_a("ABCDEF", 3, 3, "DEF");
			Test03_a("ABCDEF", 0, 6, "ABCDEF");
			Test03_a("ABCDEF", 0, 0, "");

			Test03_b("ABCDEF", 6, "");
			Test03_b("ABCDEF", 3, "DEF");
			Test03_b("ABCDEF", 0, "ABCDEF");

			Console.WriteLine("OK!");
		}

		private void Test03_a(string sSrc, int offset, int size, string sAssumeDest)
		{
			char[] src = sSrc.ToArray();
			char[] assumeDest = sAssumeDest.ToArray();

			char[] dest = SCommon.GetPart(src, offset, size);

			if (SCommon.Comp(dest, assumeDest, (a, b) => (int)a - (int)b) != 0)
				throw null;

			Console.WriteLine("OK");
		}

		private void Test03_b(string sSrc, int offset, string sAssumeDest)
		{
			char[] src = sSrc.ToArray();
			char[] assumeDest = sAssumeDest.ToArray();

			char[] dest = SCommon.GetPart(src, offset);

			if (SCommon.Comp(dest, assumeDest, (a, b) => (int)a - (int)b) != 0)
				throw null;

			Console.WriteLine("OK");
		}

		public void Test04()
		{
			Exception ex = SCommon.ToThrow(() => SCommon.Hex.I.ToBytes("xxx"));

			Console.WriteLine("" + ex);

			MessageBox.Show(ex.Message, "AAAA / Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
		}

		public void Test05()
		{
			for (int c = 0; c < 10000; c++)
			{
				byte[] data = SCommon.CRandom.GetBytes(SCommon.CRandom.GetInt(1000));
				string str = SCommon.Hex.I.ToString(data);
				byte[] retData = SCommon.Hex.I.ToBytes(str);

				if (SCommon.Comp(data, retData) != 0)
					throw null;
			}
			Console.WriteLine("OK!");
		}

		public void Test06()
		{
			Test06_a();
			Test06_b();
			Test06_b2();

			Console.WriteLine("OK!");
		}

		private void Test06_a()
		{
			for (int c = 0; c < 10000; c++)
			{
				byte[] data = SCommon.CRandom.GetBytes(SCommon.CRandom.GetInt(1000));
				string str = SCommon.Base64.I.Encode(data);
				byte[] retData = SCommon.Base64.I.Decode(str);

				if (!Regex.IsMatch(str, @"^[A-Za-z0-9\+/]*={0,2}$"))
					throw null;

				if (str.Length % 4 != 0)
					throw null;

				if (SCommon.Comp(data, retData) != 0)
					throw null;
			}
			Console.WriteLine("OK");
		}

		private void Test06_b()
		{
			char[] TEST_CHARS = (SCommon.HALF + SCommon.MBC_ASCII + "いろはにほへと日本語漢字").ToArray();

			for (int c = 0; c < 10000; c++)
			{
				string str = new string(Enumerable.Range(0, SCommon.CRandom.GetInt(1000)).Select(dummy => SCommon.CRandom.ChooseOne(TEST_CHARS)).ToArray());

				// でたらめな文字列でも例外を投げずに何らかのバイト列を返すはず。
				//
				byte[] retData = SCommon.Base64.I.Decode(str);

				if (retData == null)
					throw null;
			}
			Console.WriteLine("OK");
		}

		private void Test06_b2()
		{
			for (int c = 0; c < 10000; c++)
			{
				string str = new string(Enumerable.Range(0, SCommon.CRandom.GetInt(1000)).Select(dummy => (char)SCommon.CRandom.GetInt(0x10000)).ToArray());

				// でたらめな文字列でも例外を投げずに何らかのバイト列を返すはず。
				//
				byte[] retData = SCommon.Base64.I.Decode(str);

				if (retData == null)
					throw null;
			}
			Console.WriteLine("OK");
		}

		public void Test07()
		{
			HashSet<string> hs = SCommon.CreateSet();

			byte[] data = new byte[0];

			while (data.Length < 4)
			{
				// Increment
				{
					for (int index = 0; ; index++)
					{
						if (data.Length <= index)
						{
							data = data.Concat(new byte[1] { 0 }).ToArray();
							break;
						}
						if (data[index] < 255)
						{
							data[index]++;
							break;
						}
						data[index] = 0;
					}
				}

				//Console.WriteLine(SCommon.Hex.I.ToString(data) + " ==> " + SCommon.Base64.I.Encode(data));

				string str = SCommon.Base64.I.Encode(data);
				str = str.Substring(str.Length - 4);

				hs.Add(str);
			}

			//File.WriteAllLines(SCommon.NextOutputPath() + ".txt", hs.ToArray(), Encoding.ASCII);

			HashSet<string> h21 = SCommon.CreateSet();
			HashSet<string> h22 = SCommon.CreateSet();
			HashSet<string> h31 = SCommon.CreateSet();
			HashSet<string> h32 = SCommon.CreateSet();
			HashSet<string> h33 = SCommon.CreateSet();
			HashSet<string> h41 = SCommon.CreateSet();
			HashSet<string> h42 = SCommon.CreateSet();
			HashSet<string> h43 = SCommon.CreateSet();
			HashSet<string> h44 = SCommon.CreateSet();

			foreach (string str in hs)
			{
				if (str.EndsWith("=="))
				{
					h21.Add("" + str[0]);
					h22.Add("" + str[1]);
				}
				else if (str.EndsWith("="))
				{
					h31.Add("" + str[0]);
					h32.Add("" + str[1]);
					h33.Add("" + str[2]);
				}
				else
				{
					h41.Add("" + str[0]);
					h42.Add("" + str[1]);
					h43.Add("" + str[2]);
					h44.Add("" + str[3]);
				}
			}

			Test07_a("h21", h21);
			Test07_a("h22", h22);
			Test07_a("h31", h31);
			Test07_a("h32", h32);
			Test07_a("h33", h33);
			Test07_a("h41", h41);
			Test07_a("h42", h42);
			Test07_a("h43", h43);
			Test07_a("h44", h44);
		}

		private void Test07_a(string title, HashSet<string> hs)
		{
			Console.WriteLine(title + " ==> " + string.Join("", hs.OrderBy(SCommon.Comp)));
		}

		public void Test08()
		{
			Test08_a();
			Test08_b();
			Test08_b2();

			Console.WriteLine("OK!");
		}

		private void Test08_a()
		{
			for (int c = 0; c < 10000; c++)
			{
				byte[] data = SCommon.CRandom.GetBytes(SCommon.CRandom.GetInt(1000));
				string str = SCommon.Base32.I.Encode(data);
				byte[] retData = SCommon.Base32.I.Decode(str);

				if (!Regex.IsMatch(str, @"^[A-Z2-7]*={0,6}$"))
					throw null;

				if (str.Length % 8 != 0)
					throw null;

				if (SCommon.Comp(data, retData) != 0)
					throw null;
			}
			Console.WriteLine("OK");
		}

		private void Test08_b()
		{
			char[] TEST_CHARS = (SCommon.HALF + SCommon.MBC_ASCII + "いろはにほへと日本語漢字").ToArray();

			for (int c = 0; c < 10000; c++)
			{
				string str = new string(Enumerable.Range(0, SCommon.CRandom.GetInt(1000)).Select(dummy => SCommon.CRandom.ChooseOne(TEST_CHARS)).ToArray());

				// でたらめな文字列でも例外を投げずに何らかのバイト列を返すはず。
				//
				byte[] retData = SCommon.Base32.I.Decode(str);

				if (retData == null)
					throw null;
			}
			Console.WriteLine("OK");
		}

		private void Test08_b2()
		{
			for (int c = 0; c < 10000; c++)
			{
				string str = new string(Enumerable.Range(0, SCommon.CRandom.GetInt(1000)).Select(dummy => (char)SCommon.CRandom.GetInt(0x10000)).ToArray());

				// でたらめな文字列でも例外を投げずに何らかのバイト列を返すはず。
				//
				byte[] retData = SCommon.Base32.I.Decode(str);

				if (retData == null)
					throw null;
			}
			Console.WriteLine("OK");
		}

		public void Test09()
		{
			Test09_a("foobar", "MZXW6YTBOI======");
			Test09_a("fooba", "MZXW6YTB");
			Test09_a("foob", "MZXW6YQ=");
			Test09_a("foo", "MZXW6===");
			Test09_a("fo", "MZXQ====");
			Test09_a("f", "MY======");
			Test09_a("", "");
			Test09_a("      ", "EAQCAIBAEA======");
			Test09_a("     ", "EAQCAIBA");
			Test09_a("    ", "EAQCAIA=");
			Test09_a("   ", "EAQCA===");
			Test09_a("  ", "EAQA====");
			Test09_a(" ", "EA======");

			Console.WriteLine("OK!");
		}

		private void Test09_a(string asciiString, string assumeBase32)
		{
			Console.WriteLine("< {" + asciiString + "}");
			Console.WriteLine("A " + assumeBase32);

			string base32 = SCommon.Base32.I.Encode(Encoding.ASCII.GetBytes(asciiString));

			Console.WriteLine("> " + base32);

			if (base32 != assumeBase32)
				throw null;

			Console.WriteLine("OK");
		}

		public void Test10()
		{
			char[] TEST_CHARS = (SCommon.HALF + SCommon.MBC_ASCII + "いろはにほへと日本語漢字").ToArray();

			for (int index = 0; index < 1000; index++)
			{
				if (index % 100 == 0) Console.WriteLine("" + index); // cout

				string[] lines = Enumerable.Range(0, SCommon.CRandom.GetInt(300))
					.Select(dummy1 => new string(Enumerable.Range(0, SCommon.CRandom.GetInt(300)).Select(dummy2 => SCommon.CRandom.ChooseOne(TEST_CHARS)).ToArray()))
					.ToArray();

				string str = SCommon.Serializer.I.Join(lines);
				string[] retLines = SCommon.Serializer.I.Split(str);

				if (SCommon.Comp(lines, retLines, SCommon.Comp) != 0)
					throw null;
			}
			Console.WriteLine("OK!");
		}
	}
}
