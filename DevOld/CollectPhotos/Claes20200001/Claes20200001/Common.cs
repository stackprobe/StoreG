using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Drawing.Imaging;
using Charlotte.Commons;
using Charlotte.Drawings;

namespace Charlotte
{
	public static class Common
	{
		/// <summary>
		/// サイズを(アスペクト比を維持して)矩形領域いっぱいに広げる。
		/// </summary>
		/// <param name="size">サイズ</param>
		/// <param name="rect">矩形領域</param>
		/// <param name="interior">矩形領域の内側に張り付く場合の出力先</param>
		/// <param name="exterior">矩形領域の外側に張り付く場合の出力先</param>
		public static void AdjustRect(D2Size size, D4Rect rect, out D4Rect interior, out D4Rect exterior)
		{
			double w_h = (rect.H * size.W) / size.H; // 高さを基準にした幅
			double h_w = (rect.W * size.H) / size.W; // 幅を基準にした高さ

			D4Rect rect1;
			D4Rect rect2;

			rect1.L = rect.L + (rect.W - w_h) / 2.0;
			rect1.T = rect.T;
			rect1.W = w_h;
			rect1.H = rect.H;

			rect2.L = rect.L;
			rect2.T = rect.T + (rect.H - h_w) / 2.0;
			rect2.W = rect.W;
			rect2.H = h_w;

			if (w_h < rect.W)
			{
				interior = rect1;
				exterior = rect2;
			}
			else
			{
				interior = rect2;
				exterior = rect1;
			}
		}

		public enum ExitRoate_e
		{
			DEGREE_000, // 回転無し
			DEGREE_090, // 時計回りに  90 度回転している。-- 正しい方向にするには時計回りに 270 度回転する。
			DEGREE_180, // 時計回りに 180 度回転している。-- 正しい方向にするには時計回りに 180 度回転する。
			DEGREE_270, // 時計回りに 270 度回転している。-- 正しい方向にするには時計回りに  90 度回転する。
		};

		public static ExitRoate_e TryGetExifRotate(string file)
		{
			try
			{
				using (Bitmap bmp = new Bitmap(file))
				{
					foreach (PropertyItem pi in bmp.PropertyItems)
					{
						if (pi.Id == 0x0112)
						{
							switch (pi.Value[0])
							{
								case 8: return ExitRoate_e.DEGREE_090;
								case 3: return ExitRoate_e.DEGREE_180;
								case 6: return ExitRoate_e.DEGREE_270;
							}
							break;
						}
					}
				}
			}
			catch
			{ }

			return ExitRoate_e.DEGREE_000;
		}
	}
}
