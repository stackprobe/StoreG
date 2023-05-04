using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Charlotte.Commons;

namespace Charlotte.Tests
{
	public class Test0003
	{
		public void Test01()
		{
			Test01_a(1, 16, "A");
			Test01_a(1, 16, "B");
			Test01_a(1, 16, "C");
			Test01_a(2, 256, "A");
			Test01_a(2, 256, "B");
			Test01_a(2, 256, "C");
			Test01_a(3, 4096, "A");
			Test01_a(3, 4096, "B");
			Test01_a(3, 4096, "C");
			Test01_a(4, 65536, "A");
			Test01_a(4, 65536, "B");
			Test01_a(4, 65536, "C");
		}

		private void Test01_a(int hLen, int denom, string shufflePtn)
		{
			HashSet<string> hs = SCommon.CreateSet();

			for (int c = 0; c < denom; c++)
			{
				hs.Add(SCommon.Hex.I.ToString(SCommon.GetSHA512(Encoding.UTF8.GetBytes(shufflePtn + c))).Substring(0, hLen));
			}
			int numer = hs.Count;
			Console.WriteLine(string.Join(", ", hLen, shufflePtn, numer, denom, ((double)numer / denom).ToString("F9")));
		}

		public void Test02()
		{
			Test02_a(4, 65536, 1 * 65536, "A");
			Test02_a(4, 65536, 1 * 65536, "B");
			Test02_a(4, 65536, 1 * 65536, "C");

			Test02_a(4, 65536, 2 * 65536, "A");
			Test02_a(4, 65536, 2 * 65536, "B");
			Test02_a(4, 65536, 2 * 65536, "C");

			Test02_a(4, 65536, 3 * 65536, "A");
			Test02_a(4, 65536, 3 * 65536, "B");
			Test02_a(4, 65536, 3 * 65536, "C");

			Test02_a(4, 65536, 4 * 65536, "A");
			Test02_a(4, 65536, 4 * 65536, "B");
			Test02_a(4, 65536, 4 * 65536, "C");
		}

		private void Test02_a(int hLen, int hDenom, int collCnt, string shufflePtn)
		{
			HashSet<string> hs = SCommon.CreateSet();

			for (int c = 0; c < collCnt; c++)
			{
				hs.Add(SCommon.Hex.I.ToString(SCommon.GetSHA512(Encoding.UTF8.GetBytes(shufflePtn + c))).Substring(0, hLen));
			}
			int numer = hs.Count;
			Console.WriteLine(string.Join(", ", hLen, shufflePtn, collCnt, numer, hDenom, ((double)numer / hDenom).ToString("F9")));
		}
	}
}
