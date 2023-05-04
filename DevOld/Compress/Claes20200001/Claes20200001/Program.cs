using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.IO;
using System.IO.Compression;
using System.Drawing;
using System.Drawing.Imaging;
using System.Windows.Forms;
using Charlotte.Commons;
using Charlotte.Tests;

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

			Main4(new ArgsReader(new string[] { }));
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
			catch (Exception e)
			{
				ProcMain.WriteLog(e);
			}
		}

		private void Main5(ArgsReader ar)
		{
			if (ar.ArgIs("/D")) // 展開
			{
				string rFile = ar.NextArg();
				string wDir = ar.NextArg();

				ar.End();

				Decompress(rFile, wDir);
			}
			else // 圧縮
			{
				string rDir = ar.NextArg();
				string wFile = ar.NextArg();

				ar.End();

				Compress(rDir, wFile);
			}
		}

		/// <summary>
		/// 圧縮用出力ストリーム
		/// </summary>
		private Stream Writer;

		/// <summary>
		/// 圧縮
		/// </summary>
		/// <param name="rDir">入力フォルダ</param>
		/// <param name="wFile">出力ファイル</param>
		private void Compress(string rDir, string wFile)
		{
			rDir = SCommon.MakeFullPath(rDir);
			wFile = SCommon.MakeFullPath(wFile);

			Console.WriteLine("圧縮します...");
			Console.WriteLine("< " + rDir);
			Console.WriteLine("> " + wFile);

			if (!Directory.Exists(rDir))
				throw new Exception("入力フォルダが見つかりません。");

			using (FileStream fileWriter = new FileStream(wFile, FileMode.Create, FileAccess.Write))
			using (GZipStream gz = new GZipStream(fileWriter, CompressionMode.Compress, true))
			{
				this.Writer = gz;

				foreach (string dir in Directory.GetDirectories(rDir, "*", SearchOption.AllDirectories))
				{
					Console.WriteLine("< " + dir);

					string relPath = SCommon.ChangeRoot(dir, rDir);

					this.WriteString("D"); // Directory
					this.WriteString(relPath);
				}
				foreach (string file in Directory.GetFiles(rDir, "*", SearchOption.AllDirectories))
				{
					Console.WriteLine("< " + file);

					string relPath = SCommon.ChangeRoot(file, rDir);

					this.WriteString("F"); // File
					this.WriteString(relPath);

					FileInfo fileInfo = new FileInfo(file);

					this.WriteString(new SCommon.SimpleDateTime(fileInfo.CreationTime).ToTimeStamp().ToString());
					this.WriteString(new SCommon.SimpleDateTime(fileInfo.LastWriteTime).ToTimeStamp().ToString());
					this.WriteString(new SCommon.SimpleDateTime(fileInfo.LastAccessTime).ToTimeStamp().ToString());
					this.WriteString(fileInfo.Length.ToString());

					using (FileStream reader = new FileStream(file, FileMode.Open, FileAccess.Read))
					{
						SCommon.ReadToEnd(reader.Read, this.Writer.Write);
					}
				}
				this.WriteString("E"); // End
				this.Writer = null;
			}
			Console.WriteLine("圧縮しました。");
		}

		/// <summary>
		/// 圧縮用文字列出力
		/// </summary>
		/// <param name="str">文字列</param>
		private void WriteString(string str)
		{
			byte[] bStr = Encoding.UTF8.GetBytes(str);

			this.Write(SCommon.IntToBytes(bStr.Length));
			this.Write(bStr);
		}

		/// <summary>
		/// 圧縮用バイト列出力
		/// </summary>
		/// <param name="data">バイト列</param>
		private void Write(byte[] data)
		{
			SCommon.Write(this.Writer, data);
		}

		/// <summary>
		/// 展開用入力ストリーム
		/// </summary>
		private Stream Reader;

		/// <summary>
		/// 展開
		/// </summary>
		/// <param name="rFile">入力ファイル</param>
		/// <param name="wDir">出力フォルダ</param>
		private void Decompress(string rFile, string wDir)
		{
			rFile = SCommon.MakeFullPath(rFile);
			wDir = SCommon.MakeFullPath(wDir);

			Console.WriteLine("展開します...");
			Console.WriteLine("< " + rFile);
			Console.WriteLine("> " + wDir);

			if (!File.Exists(rFile))
				throw new Exception("入力ファイルが見つかりません。");

			SCommon.CreateDir(wDir);

			using (FileStream fileReader = new FileStream(rFile, FileMode.Open, FileAccess.Read))
			using (GZipStream gz = new GZipStream(fileReader, CompressionMode.Decompress, true))
			{
				this.Reader = gz;

				for (; ; )
				{
					string label = this.ReadString();

					if (label == "D") // Directory
					{
						string relPath = this.ReadString();
						relPath = SCommon.ToFairRelPath(relPath, wDir.Length);
						string dir = Path.Combine(wDir, relPath);

						Console.WriteLine("> " + dir);

						SCommon.CreateDir(dir);
					}
					else if (label == "F") // File
					{
						string relPath = this.ReadString();
						relPath = SCommon.ToFairRelPath(relPath, wDir.Length);
						string file = Path.Combine(wDir, relPath);

						Console.WriteLine("> " + file);

						DateTime creationTime = ToDateTimeForFileTime(this.ReadString());
						DateTime lastWriteTime = ToDateTimeForFileTime(this.ReadString());
						DateTime lastAccessTime = ToDateTimeForFileTime(this.ReadString());
						long fileSize = long.Parse(this.ReadString());

						Console.WriteLine(creationTime);
						Console.WriteLine(lastWriteTime);
						Console.WriteLine(lastAccessTime);
						Console.WriteLine(fileSize);

						if (fileSize < 0L || SCommon.IMAX_64 < fileSize)
							throw new Exception("Bad fileSize: " + fileSize);

						using (FileStream writer = new FileStream(file, FileMode.Create, FileAccess.Write))
						{
							for (long count = 0L; count < fileSize; )
							{
								int size = (int)Math.Min(2000000, fileSize - count);
								SCommon.Write(writer, SCommon.Read(this.Reader, size));
								count += size;
							}
						}

						{
							FileInfo fileInfo = new FileInfo(file);

							fileInfo.CreationTime = creationTime;
							fileInfo.LastWriteTime = lastWriteTime;
							fileInfo.LastAccessTime = lastAccessTime;
						}
					}
					else if (label == "E") // End
					{
						break;
					}
					else
					{
						throw new Exception("不明なラベル");
					}
				}
				this.Reader = null;
			}
			Console.WriteLine("展開しました。");
		}

		/// <summary>
		/// 展開用文字列入力
		/// </summary>
		/// <returns>文字列</returns>
		private string ReadString()
		{
			int size = SCommon.ToInt(this.Read(4));

			if (size < 0 || SCommon.IMAX < size) // rough limit
				throw new Exception("Bad size: " + size);

			byte[] bStr = this.Read(size);
			string str = Encoding.UTF8.GetString(bStr);
			return str;
		}

		/// <summary>
		/// 展開用バイト列入力
		/// </summary>
		/// <param name="size">バイト列のサイズ</param>
		/// <returns>バイト列</returns>
		private byte[] Read(int size)
		{
			return SCommon.Read(this.Reader, size);
		}

		private static DateTime ToDateTimeForFileTime(string str)
		{
			SCommon.SimpleDateTime dt = SCommon.SimpleDateTime.FromTimeStamp(long.Parse(str));

			// 下限：
			// -- Win32 FileTime の下限 -- 1601/01/01 00:00:00 UTC == 1601/01/01 09:00:00 JST
			// 上限：
			// -- DateTime の上限 -- 9999/12/31 23:59:59

			if (dt.Year < 1602 || 9999 < dt.Year)
				throw new Exception("Bad date-time: " + dt);

			return dt.ToDateTime();
		}
	}
}
