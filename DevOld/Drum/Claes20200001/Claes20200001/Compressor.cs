﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.IO.Compression;
using Charlotte.Commons;

namespace Charlotte
{
	public static class Compressor
	{
		private static Stream P_Writer;

		private class DirEntry
		{
			public string LocalName;
			public List<DirEntry> DirList;
			public List<FileEntry> FileList;

			public DirEntry(string rDir, string localName)
			{
				ProcMain.WriteLog("D " + localName);

				this.LocalName = localName;
				this.DirList = Directory.GetDirectories(rDir)
					.OrderBy(SCommon.CompIgnoreCase)
					.Select(dirPath => new DirEntry(dirPath, Path.GetFileName(dirPath)))
					.ToList();
				this.FileList = Directory.GetFiles(rDir)
					.OrderBy(SCommon.CompIgnoreCase)
					.Select(filePath => new FileEntry(filePath))
					.ToList();

				ProcMain.WriteLog("d-done");
			}

			public static List<FileEntry> P_CollectedFiles;

			public void CollectFiles()
			{
				foreach (DirEntry dir in this.DirList)
					dir.CollectFiles();

				foreach (FileEntry file in this.FileList)
					P_CollectedFiles.Add(file);
			}

			public void Write()
			{
				P_Writer.WriteByte(CompressorConsts.SIGNATURE_DIR_ENTRY_START);
				SCommon.WritePartString(P_Writer, this.LocalName);
				this.Write_DFs();
			}

			public void Write_DFs()
			{
				foreach (DirEntry dir in this.DirList)
					dir.Write();

				foreach (FileEntry file in this.FileList)
					file.Write();

				P_Writer.WriteByte(CompressorConsts.SIGNATURE_DIR_ENTRY_END);
			}
		}

		private class FileEntry
		{
			public string LocalName;
			public string FullPath;
			public SCommon.SimpleDateTime CreationTime;
			public SCommon.SimpleDateTime LastWriteTime;
			public SCommon.SimpleDateTime LastAccessTime;
			public long Length;

			public FileEntry(string filePath)
			{
				string localName = Path.GetFileName(filePath);
				ProcMain.WriteLog("F " + localName);
				FileInfo info = new FileInfo(filePath);
				long length = info.Length;
				ProcMain.WriteLog("L " + length);

				this.LocalName = localName;
				this.FullPath = filePath;
				this.CreationTime = new SCommon.SimpleDateTime(info.CreationTime);
				this.LastWriteTime = new SCommon.SimpleDateTime(info.LastWriteTime);
				this.LastAccessTime = new SCommon.SimpleDateTime(info.LastAccessTime);
				this.Length = length;

				ProcMain.WriteLog("f-done");
			}

			public string Hash;

			public void CalculateHash()
			{
				this.Hash = SCommon.Hex.I.ToString(SCommon.GetSHA512File(this.FullPath));
			}

			public void Write()
			{
				P_Writer.WriteByte(CompressorConsts.SIGNATURE_FILE_ENTRY_START);
				SCommon.WritePartString(P_Writer, this.LocalName);
				SCommon.WritePartLong(P_Writer, this.CreationTime.ToTimeStamp());
				SCommon.WritePartLong(P_Writer, this.LastWriteTime.ToTimeStamp());
				SCommon.WritePartLong(P_Writer, this.LastAccessTime.ToTimeStamp());
				SCommon.WritePartString(P_Writer, this.Hash);
			}
		}

		public static void Perform(string rDir, string wFile)
		{
			rDir = SCommon.MakeFullPath(rDir);
			wFile = SCommon.MakeFullPath(wFile);

			Console.WriteLine("< " + rDir);
			Console.WriteLine("> " + wFile);

			if (!Directory.Exists(rDir))
				throw new ArgumentException("rDir is not directory");

			if (Directory.Exists(wFile))
				throw new ArgumentException("wFile is directory");

			// 引数チェックここまで

			File.WriteAllBytes(wFile, SCommon.EMPTY_BYTES); // 出力テスト

			DirEntry root = new DirEntry(rDir, null);

			DirEntry.P_CollectedFiles = new List<FileEntry>();
			root.CollectFiles();
			List<FileEntry> files = DirEntry.P_CollectedFiles;
			DirEntry.P_CollectedFiles = null;

			for (int index = 0; index < files.Count; index++)
			{
				FileEntry file = files[index];

				ProcMain.WriteLog("calc-hash " + index + " / " + files.Count + " = " + ((double)index / files.Count).ToString("F9"));
				ProcMain.WriteLog("F " + file.LocalName);
				ProcMain.WriteLog("L " + file.Length);

				file.CalculateHash();

				ProcMain.WriteLog("H " + file.Hash);
			}
			ProcMain.WriteLog("calc-hash-end");

			Dictionary<string, FileEntry> hash2file = SCommon.CreateDictionary<FileEntry>();

			foreach (FileEntry file in files)
			{
				if (hash2file.ContainsKey(file.Hash))
				{
					FileEntry mvFile = hash2file[file.Hash];

					// ? ハッシュ値が同じなのにファイルの長さが異なる。-> ハッシュ値のコリジョン
					// -- 確率的にあり得ないので多分何かのバグ
					// -- 本当にコリジョンだったら大発見 @ 2022.4.23
					if (mvFile.Length != file.Length)
						throw new Exception("Collision ???");
				}
				else
				{
					hash2file.Add(file.Hash, file);
				}
			}

			using (FileStream fileWriter = new FileStream(wFile, FileMode.Create, FileAccess.Write))
			using (GZipStream writer = new GZipStream(fileWriter, CompressionMode.Compress, true))
			{
				P_Writer = writer;
				root.Write_DFs();
				P_Writer = null;

				foreach (string hash in hash2file.Keys.OrderBy(SCommon.Comp))
				{
					FileEntry file = hash2file[hash];

					ProcMain.WriteLog("H " + hash);
					ProcMain.WriteLog("L " + file.Length);
					ProcMain.WriteLog("< " + file.FullPath);

					writer.WriteByte(CompressorConsts.SIGNATURE_FILE_DATA_START);
					SCommon.WritePartString(writer, hash);
					SCommon.WritePartLong(writer, file.Length);

					using (FileStream reader = new FileStream(file.FullPath, FileMode.Open, FileAccess.Read))
					{
						SCommon.ReadToEnd(reader.Read, writer.Write);
					}
					ProcMain.WriteLog("done");
				}
				writer.WriteByte(CompressorConsts.SIGNATURE_END);
			}
			Console.WriteLine("done!");
		}
	}
}
