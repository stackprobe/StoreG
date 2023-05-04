using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Drawing.Drawing2D;
using Charlotte.Commons;
using Charlotte.Drawings;
using Charlotte.Utilities;

namespace Charlotte.Tests
{
	public class Test0001
	{
		public void Test01()
		{
			Canvas canvas = Canvas.LoadFromFile(@"C:\temp\yande.re_268454.jpg");

			canvas = canvas.GetSubImage(new I4Rect(0, 160, 2560, 1440));
			canvas = canvas.Expand(1920, 1080);

			canvas.Save(SCommon.NextOutputPath() + ".png");

			// ----

			canvas = Canvas.LoadFromFile(@"C:\temp\yande.re_268455.jpg");

			canvas = canvas.GetSubImage(new I4Rect(0, 0, 2560, 1440));
			canvas = canvas.Expand(1920, 1080);

			canvas.Save(SCommon.NextOutputPath() + ".png");
		}

		public void Test02()
		{
			using (Bitmap bmp = (Bitmap)Bitmap.FromFile(@"C:\temp\FqbOpZMaYAEtPlo.jpg"))
			{
				Test02_a(bmp, -300);
				Test02_a(bmp, -200);
				Test02_a(bmp, -100);
				Test02_a(bmp, 0);
				Test02_a(bmp, 100);
				Test02_a(bmp, 200);
				Test02_a(bmp, 300);
			}
		}

		private void Test02_a(Bitmap bmp, int sizeAdd)
		{
			int w = bmp.Width;
			int h = bmp.Height;

			int new_w = w + sizeAdd;
			int new_h = h + sizeAdd;

			using (Bitmap dest = new Bitmap(new_w, new_h))
			{
				using (Graphics g = Graphics.FromImage(dest))
				{
					g.InterpolationMode = InterpolationMode.HighQualityBicubic;
					g.DrawImage(bmp, new Rectangle(0, 0, new_w, new_h));
				}
				dest.Save(SCommon.NextOutputPath() + ".png");
			}
		}

		public void Test03()
		{
			using (Bitmap bmp = (Bitmap)Bitmap.FromFile(@"C:\temp\FqbOpZMaYAEtPlo.jpg"))
			{
				Test03_a(bmp, -300);
				Test03_a(bmp, -200);
				Test03_a(bmp, -100);
				Test03_a(bmp, 0);
				Test03_a(bmp, 100);
				Test03_a(bmp, 200);
				Test03_a(bmp, 300);
			}
		}

		private void Test03_a(Bitmap bmp, int sizeAdd)
		{
			int w = bmp.Width;
			int h = bmp.Height;

			int new_w = w + sizeAdd;
			int new_h = h + sizeAdd;

			using (WorkingDir wd = new WorkingDir())
			{
				string file1 = wd.MakePath();
				string file2 = wd.MakePath();

				using (Bitmap dest = new Bitmap(new_w, new_h))
				{
					using (Graphics g = Graphics.FromImage(dest))
					{
						g.InterpolationMode = InterpolationMode.HighQualityBicubic;
						g.DrawImage(bmp, new Rectangle(0, 0, new_w, new_h));
					}
					dest.Save(file1);
				}

				Canvas canvas = Canvas.Load(bmp);
				canvas = canvas.Expand(new_w, new_h);
				canvas.Save(file2);

				// ----

				Canvas c1 = Canvas.LoadFromFile(file1);
				Canvas c2 = Canvas.LoadFromFile(file2);

				int diffMax = 0;

				for (int x = 0; x < new_w; x++)
				{
					for (int y = 0; y < new_h; y++)
					{
						diffMax = Math.Max(diffMax, Math.Abs(c1[x, y].R - c2[x, y].R));
						diffMax = Math.Max(diffMax, Math.Abs(c1[x, y].G - c2[x, y].G));
						diffMax = Math.Max(diffMax, Math.Abs(c1[x, y].B - c2[x, y].B));
					}
				}
				Console.WriteLine("diffMax: " + diffMax);
			}
		}

		public void Test04()
		{
			using (Bitmap bmp = (Bitmap)Bitmap.FromFile(@"C:\temp\FqbOpZMaYAEtPlo.jpg"))
			{
				Test04_a(bmp, -300);
				Test04_a(bmp, -200);
				Test04_a(bmp, -100);
				Test04_a(bmp, 0);
				Test04_a(bmp, 100);
				Test04_a(bmp, 200);
				Test04_a(bmp, 300);
			}
		}

		private void Test04_a(Bitmap bmp, int sizeAdd)
		{
			int w = bmp.Width;
			int h = bmp.Height;

			int new_w = w + sizeAdd;
			int new_h = h + sizeAdd;

			using (WorkingDir wd = new WorkingDir())
			{
				string file1 = wd.MakePath();
				string file2 = wd.MakePath();

				using (Bitmap dest = new Bitmap(new_w, new_h))
				{
					using (Graphics g = Graphics.FromImage(dest))
					{
						g.InterpolationMode = InterpolationMode.HighQualityBicubic;
						g.DrawImage(bmp, new Rectangle(0, 0, new_w, new_h));
					}
					dest.Save(file1);
				}

				Canvas canvas = Canvas.Load(bmp);
				canvas = canvas.Expand(new_w, new_h);
				canvas.Save(file2);

				// ----

				Canvas c1 = Canvas.LoadFromFile(file1);
				Canvas c2 = Canvas.LoadFromFile(file2);
				Canvas c3 = new Canvas(new_w, new_h);

				const int COLOR_DIFF_MUL = 6;

				Dictionary<int, int> map = new Dictionary<int, int>();

				for (int x = 0; x < new_w; x++)
				{
					for (int y = 0; y < new_h; y++)
					{
						int diff = 0;

						diff = Math.Max(diff, Math.Abs(c1[x, y].R - c2[x, y].R));
						diff = Math.Max(diff, Math.Abs(c1[x, y].G - c2[x, y].G));
						diff = Math.Max(diff, Math.Abs(c1[x, y].B - c2[x, y].B));

						if (map.ContainsKey(diff))
							map[diff]++;
						else
							map[diff] = 1;

						int level = diff * COLOR_DIFF_MUL;
						level = Math.Min(level, 255);
						c3[x, y] = new I4Color(level, level, level, 255);
					}
				}

				int[][] ranking = map.Select(kv => new int[] { kv.Key, kv.Value }).ToArray();

				Array.Sort(ranking, (a, b) => (a[1] - b[1]) * -1);

				foreach (int[] kv in ranking)
				{
					Console.WriteLine(string.Format("diff: {0} == count: {1}", kv[0], kv[1]));
				}

				Console.WriteLine("diff < 8 ==> " + ((double)map.Where(kv => kv.Key < 8).Count() / (new_w * new_h)));
				Console.WriteLine("diff < 16 ==> " + ((double)map.Where(kv => kv.Key < 16).Count() / (new_w * new_h)));
				Console.WriteLine("diff < 24 ==> " + ((double)map.Where(kv => kv.Key < 24).Count() / (new_w * new_h)));
				Console.WriteLine("diff < 32 ==> " + ((double)map.Where(kv => kv.Key < 32).Count() / (new_w * new_h)));
				Console.WriteLine("diff < 40 ==> " + ((double)map.Where(kv => kv.Key < 40).Count() / (new_w * new_h)));

				c1.Save(SCommon.NextOutputPath() + ".png");
				c2.Save(SCommon.NextOutputPath() + ".png");
				c3.Save(SCommon.NextOutputPath() + ".png");
			}
		}
	}
}
