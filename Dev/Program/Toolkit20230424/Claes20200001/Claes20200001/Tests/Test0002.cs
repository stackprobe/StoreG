﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Charlotte.Commons;
using Charlotte.Drawings;
using Charlotte.Utilities;

namespace Charlotte.Tests
{
	class Test0002
	{
		private const string INPUT_ROOT_DIR = @"C:\temp";

		private static string[] IMAGE_EXTS = new string[]
		{
			".bmp",
			".jpg",
			".jpeg",
			".png",
		};

		private static I2Size MONITOR_SIZE = new I2Size(1920, 1080);

		// ----

		private string PictureName;
		private Canvas Picture;
		private Canvas Picture_I;
		private Canvas Picture_E;
		private I4Rect Interior;
		private I4Rect Exterior;
		private bool InteriorExpanded;
		private bool ExteriorExpanded;

		public void Test01()
		{
			foreach (string file in Directory.GetFiles(INPUT_ROOT_DIR))
			{
				string ext = Path.GetExtension(file);

				if (IMAGE_EXTS.Any(v => SCommon.EqualsIgnoreCase(v, ext)))
				{
					ProcMain.WriteLog("< " + file);

					PictureName = Path.GetFileNameWithoutExtension(file);
					Picture = Canvas.LoadFromFile(file);
					Picture.FilterAllDot((dot, x, y) => new I4Color(dot.R, dot.G, dot.B, 255)); // 不透明にする。

					ProcMain.WriteLog("W " + Picture.W);
					ProcMain.WriteLog("H " + Picture.H);

					D4Rect[] rects = Common.EnlargeFull(
						new I2Size(Picture.W, Picture.H).ToD2Size(),
						new I4Rect(0, 0, MONITOR_SIZE.W, MONITOR_SIZE.H).ToD4Rect()
						);

					Interior = rects[0].ToI4Rect();
					Exterior = rects[1].ToI4Rect();
					InteriorExpanded = Picture.W < Interior.W || Picture.H < Interior.H;
					ExteriorExpanded = Picture.W < Exterior.W || Picture.H < Exterior.H;

					ProcMain.WriteLog("I.L " + Interior.L);
					ProcMain.WriteLog("I.T " + Interior.T);
					ProcMain.WriteLog("I.W " + Interior.W);
					ProcMain.WriteLog("I.H " + Interior.H);
					ProcMain.WriteLog("E.L " + Exterior.L);
					ProcMain.WriteLog("E.T " + Exterior.T);
					ProcMain.WriteLog("E.W " + Exterior.W);
					ProcMain.WriteLog("E.H " + Exterior.H);
					ProcMain.WriteLog("I-X " + InteriorExpanded);
					ProcMain.WriteLog("E-X " + ExteriorExpanded);

					if (Interior.T < 5 && Interior.L < 5) // ? アスペクト比が(同じ || ほとんど同じ)
					{
						if (!ExteriorExpanded) // 拡大することになる場合、生成しない。
						{
							OutputSimple();
						}
					}
					else
					{
						Picture_I = Picture.Expand(Interior.W, Interior.H);
						Picture_E = Picture.Expand(Exterior.W, Exterior.H);

						if (!ExteriorExpanded) // 背面側について拡大した場合、背面のみの壁紙は生成しない。
						{
							OutputTopOrLeft();
							OutputBottomOrRight();
						}
						OutputCenter();

						Picture_I = null;
						Picture_E = null;
						InteriorExpanded = default(bool);
						ExteriorExpanded = default(bool);
					}

					PictureName = null;
					Picture = null;
					Interior = default(I4Rect);
					Exterior = default(I4Rect);

					ProcMain.WriteLog("done");
				}
			}
			ProcMain.WriteLog("done!");
		}

		private void OutputSimple()
		{
			Canvas canvas = Picture;
			canvas = canvas.Expand(MONITOR_SIZE.W, MONITOR_SIZE.H);
			canvas.Save(Path.Combine(SCommon.GetOutputDir(), PictureName + ".png"));
		}

		private void OutputTopOrLeft()
		{
			string suffix = Exterior.L == 0 ? "T" : "L";
			Canvas canvas = Picture_E;
			canvas = canvas.GetSubImage(new I4Rect(0, 0, MONITOR_SIZE.W, MONITOR_SIZE.H));
			canvas.Save(Path.Combine(SCommon.GetOutputDir(), PictureName + suffix + ".png"));
		}

		private void OutputBottomOrRight()
		{
			string suffix = Exterior.L == 0 ? "B" : "R";
			Canvas canvas = Picture_E;
			canvas = canvas.GetSubImage(new I4Rect(Exterior.W - MONITOR_SIZE.W, Exterior.H - MONITOR_SIZE.H, MONITOR_SIZE.W, MONITOR_SIZE.H));
			canvas.Save(Path.Combine(SCommon.GetOutputDir(), PictureName + suffix + ".png"));
		}

		private void OutputCenter()
		{
			string suffix = "C";
			Canvas canvas = Picture_E;
			canvas = canvas.GetSubImage(new I4Rect((Exterior.W - MONITOR_SIZE.W) / 2, (Exterior.H - MONITOR_SIZE.H) / 2, MONITOR_SIZE.W, MONITOR_SIZE.H));
			canvas.Save(Path.Combine(SCommon.GetOutputDir(), PictureName + suffix + ".png"));

			// ----

			if (ExteriorExpanded) // 背面側について拡大した場合、画像が荒くなるのでボカす。
				canvas.Blur(5);

			canvas.FilterAllDot((dot, x, y) => new I4Color(dot.R / 2, dot.G / 2, dot.B / 2, 255));

			// 前面を描画
			if (InteriorExpanded)
				canvas.DrawImage(Picture, (MONITOR_SIZE.W - Picture.W) / 2, (MONITOR_SIZE.H - Picture.H) / 2, false); // 拡大してしまう -> そのまま中央に描画
			else
				canvas.DrawImage(Picture_I, Interior.L, Interior.T, false); // 縮小して描画

			canvas.Save(Path.Combine(SCommon.GetOutputDir(), PictureName + ".png"));
		}
	}
}
