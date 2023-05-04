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
using Charlotte.Utilities;
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
			catch (Exception ex)
			{
				ProcMain.WriteLog(ex);

				//MessageBox.Show("" + ex, ProcMain.APP_TITLE + " / エラー", MessageBoxButtons.OK, MessageBoxIcon.Error);

				//Console.WriteLine("Press ENTER key. (エラーによりプログラムを終了します)");
				//Console.ReadLine();
			}
		}

		private enum MemoryMode_e
		{
			AUTO = 1,
			FORCE_ON_MEMORY,
			FORCE_NON_ON_MEMORY,
		}

		private void Main5(ArgsReader ar)
		{
			MemoryMode_e memoryMode = MemoryMode_e.AUTO;

			if (ar.ArgIs("/M"))
			{
				memoryMode = MemoryMode_e.FORCE_ON_MEMORY;
			}
			if (ar.ArgIs("/F"))
			{
				memoryMode = MemoryMode_e.FORCE_NON_ON_MEMORY;
			}

			byte[] rawKey;

			if (ar.ArgIs("/K"))
			{
				rawKey = ReadRawKeyFile(ar.NextArg());
			}
			else if (ar.ArgIs("/P"))
			{
				rawKey = SCommon.GetSHA512(Encoding.UTF8.GetBytes(ar.NextArg()));
			}
			else
			{
				throw new Exception("Bad command line option (not /K or /P)");
			}

			Console.WriteLine("rawKey: " + SCommon.Hex.I.ToString(rawKey));

			bool encryptMode;

			if (ar.ArgIs("/E"))
			{
				encryptMode = true;
			}
			else if (ar.ArgIs("/D"))
			{
				encryptMode = false;
			}
			else
			{
				throw new Exception("Bad command line option (not /E or /D)");
			}

			Console.WriteLine("encryptMode: " + encryptMode);

			string rFile = ar.NextArg();
			string wFile = ar.NextArg();
			string procTargFile;

			if (wFile == "*")
			{
				rFile = SCommon.ToFullPath(rFile);

				Console.WriteLine("< " + rFile);
				Console.WriteLine("> " + rFile);

				if (!File.Exists(rFile))
					throw new Exception("no rFile");

				procTargFile = rFile;
			}
			else
			{
				rFile = SCommon.ToFullPath(rFile);
				wFile = SCommon.ToFullPath(wFile);

				Console.WriteLine("< " + rFile);
				Console.WriteLine("> " + wFile);

				if (!File.Exists(rFile))
					throw new Exception("no rFile");

				if (Directory.Exists(wFile))
					throw new Exception("Bad wFile");

				if (SCommon.EqualsIgnoreCase(rFile, wFile))
					throw new Exception("rFile, wFile are same file");

				File.Copy(rFile, wFile, true);
				procTargFile = wFile;
			}

			ar.End();

			try
			{
				bool onMemory;

				if (memoryMode == MemoryMode_e.AUTO)
				{
					FileInfo fileInfo = new FileInfo(procTargFile);
					long fileSize = fileInfo.Length;

					Console.WriteLine("fileSize: " + fileSize);

					if (fileSize <= Consts.ON_MEMORY_FILE_SIZE_MAX)
					{
						onMemory = true;
					}
					else
					{
						onMemory = false;
					}
				}
				else if (memoryMode == MemoryMode_e.FORCE_ON_MEMORY)
				{
					onMemory = true;
				}
				else if (memoryMode == MemoryMode_e.FORCE_NON_ON_MEMORY)
				{
					onMemory = false;
				}
				else
				{
					throw null; // never
				}

				Console.WriteLine("onMemory: " + onMemory);
				ProcMain.WriteLog("Start");

				if (onMemory)
				{
					using (RingCipher transformer = new RingCipher(rawKey))
					{
						byte[] fileData = File.ReadAllBytes(procTargFile);

						if (encryptMode)
							fileData = transformer.Encrypt(fileData);
						else
							fileData = transformer.Decrypt(fileData);

						File.WriteAllBytes(procTargFile, fileData);
					}
				}
				else
				{
					using (RingCipherFile transformer = new RingCipherFile(rawKey))
					{
						if (encryptMode)
							transformer.Encrypt(procTargFile);
						else
							transformer.Decrypt(procTargFile);
					}
				}

				ProcMain.WriteLog("Successful");
			}
			catch
			{
				SCommon.DeletePath(procTargFile);
				throw;
			}
		}

		private byte[] ReadRawKeyFile(string file)
		{
			string[] lines = File.ReadAllLines(file, Encoding.ASCII)
				.Select(line => line.Trim()) // 行頭・行末の空白を除去
				.Where(line => line != "") // 空行を除去
				.Where(line => line[0] != ';') // コメント行を除去
				.ToArray();

			foreach (string line in lines)
				if (!Regex.IsMatch(line, "^[0-9A-Fa-f]*$") || line.Length % 2 != 0)
					throw new Exception("Bad raw-key-file line: " + line);

			string text = string.Join("", lines);

			if (
				text.Length < 32 ||
				text.Length % 16 != 0
				)
				throw new Exception("Bad raw-key-file text: " + text);

			return SCommon.Hex.I.ToBytes(text);
		}
	}
}
