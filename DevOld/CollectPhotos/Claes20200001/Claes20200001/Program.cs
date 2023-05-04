using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.IO;
using System.Drawing;
using System.Drawing.Imaging;
using System.Windows.Forms;
using Charlotte.Commons;
using Charlotte.Tests;
using Charlotte.Utilities;
using Charlotte.Drawings;

namespace Charlotte
{
	class Program
	{
		static void Main(string[] args)
		{
			ProcMain.CUIMain(new Program().Main2);
		}

		private void Main2(ArgsReader ar)
		{
			if (ProcMain.DEBUG)
			{
				Main3();
			}
			else
			{
				Main4(ar);
			}
			SCommon.OpenOutputDirIfCreated();
		}

		private void Main3()
		{
			// テスト系 -- リリース版では使用しない。
#if DEBUG
			// -- choose one --

			Main4(new ArgsReader(new string[] { @"C:\temp" }));
			//Main4(new ArgsReader(new string[] { "/F", @"C:\temp" }));
			//new Test0001().Test01();
			//new Test0002().Test01();
			//new Test0003().Test01();

			// --
#endif
			SCommon.Pause();
		}

		private void Main4(ArgsReader ar)
		{
			try
			{
				Main5(ar);
			}
			catch (Exception ex)
			{
				ProcMain.WriteLog(ex);

				MessageBox.Show("" + ex, Path.GetFileNameWithoutExtension(ProcMain.SelfFile) + " / エラー", MessageBoxButtons.OK, MessageBoxIcon.Error);

				//Console.WriteLine("Press ENTER key. (エラーによりプログラムを終了します)");
				//Console.ReadLine();
			}
		}

		private bool ForceCheckPhotoFlag;

		private void Main5(ArgsReader ar)
		{
			this.ForceCheckPhotoFlag = ar.ArgIs("/F");
			string dir = SCommon.MakeFullPath(ar.NextArg());

			ar.End();

			Console.WriteLine("dir: " + dir);

			if (!Directory.Exists(dir))
				throw new Exception("指定されたフォルダは存在しません！");

			foreach (string file in Directory.GetFiles(dir))
				this.ProcPhotoFile(file);
		}

		private void ProcPhotoFile(string file)
		{
			if (!this.IsTargetPhotoFile(file))
				return;

			string fileNew = null;

			if (!this.IsAlreadyRenamedPhotoFile(file))
			{
				FileInfo fileInfo = new FileInfo(file);
				SCommon.SimpleDateTime fileTime = new SCommon.SimpleDateTime(fileInfo.LastWriteTime);

				for (int count = 0; ; count++)
				{
					string name = string.Format(
						"{0:D4}-{1:D2}-{2:D2}_{3:D2}-{4:D2}-{5:D2}_{6:D3}.jpg",
						fileTime.Year,
						fileTime.Month,
						fileTime.Day,
						fileTime.Hour,
						fileTime.Minute,
						fileTime.Second,
						count
						);

					fileNew = Path.Combine(Path.GetDirectoryName(file), name);

					if (!File.Exists(fileNew))
						break;
				}
				Console.WriteLine("< " + file);
				Console.WriteLine("N " + fileNew);
			}
			if (fileNew != null || this.ForceCheckPhotoFlag)
			{
				Console.WriteLine("# " + file);

				Canvas canvas = Canvas.LoadFromFile(file);
				bool changed = false;

				Common.ExitRoate_e exifRotate = Common.TryGetExifRotate(file);

				switch (exifRotate)
				{
					case Common.ExitRoate_e.DEGREE_000:
						break;

					case Common.ExitRoate_e.DEGREE_090:
						canvas = canvas.Rotate90();
						canvas = canvas.Rotate90();
						canvas = canvas.Rotate90();
						changed = true;
						break;

					case Common.ExitRoate_e.DEGREE_180:
						canvas = canvas.Rotate90();
						canvas = canvas.Rotate90();
						changed = true;
						break;

					case Common.ExitRoate_e.DEGREE_270:
						canvas = canvas.Rotate90();
						changed = true;
						break;

					default:
						throw null; // never
				}

				if (Consts.DEST_PHOTO_W < canvas.W && Consts.DEST_PHOTO_H < canvas.H)
				{
					D4Rect interior;
					D4Rect exterior;

					Common.AdjustRect(
						new D2Size(canvas.W, canvas.H),
						new D4Rect(0, 0, Consts.DEST_PHOTO_W, Consts.DEST_PHOTO_H),
						out interior,
						out exterior
						);

					int w = SCommon.ToInt(exterior.W);
					int h = SCommon.ToInt(exterior.H);

					Console.WriteLine("< " + canvas.W + " x " + canvas.H);
					Console.WriteLine("> " + w + " x " + h);

					canvas = canvas.Expand(w, h);
					changed = true;
				}

				if (changed)
					canvas.SaveAsJpeg(file, 90);
			}
			if (fileNew != null)
			{
				Console.WriteLine("< " + file);
				Console.WriteLine("> " + fileNew);

				File.Move(file, fileNew);
			}
			GC.Collect();
		}

		private bool IsTargetPhotoFile(string file)
		{
			string ext = Path.GetExtension(file);
			return Consts.SRC_PHOTO_EXTS.Any(srcPhotoExt => SCommon.EqualsIgnoreCase(srcPhotoExt, ext));
		}

		private bool IsAlreadyRenamedPhotoFile(string file)
		{
			return Regex.IsMatch(Path.GetFileName(file), "^[0-9]{4}-[0-9]{2}-[0-9]{2}_[0-9]{2}-[0-9]{2}-[0-9]{2}_[0-9]{3}.jpg$", RegexOptions.IgnoreCase);
		}
	}
}
