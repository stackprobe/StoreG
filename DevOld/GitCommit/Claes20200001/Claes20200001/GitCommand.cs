using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.IO;
using Charlotte.Commons;

namespace Charlotte
{
	public static class GitCommand
	{
		private static string _gitProgram = null;

		public static string GetGitProgram()
		{
			if (_gitProgram == null)
				_gitProgram = GetGitProgram_Main();

			return _gitProgram;
		}

		private static string GetGitProgram_Main()
		{
			const string PATH_00_ENV = "USERPROFILE";
			const string PATH_01 = @"AppData\Local\GitHubDesktop";
			const string PATH_02_REGEX = "^app-[0-9]{1,9}\\.[0-9]{1,9}\\.[0-9]{1,9}$";
			const string PATH_03 = @"resources\app\git\cmd\git.exe";

			string dir_00 = Environment.GetEnvironmentVariable(PATH_00_ENV);

			if (string.IsNullOrEmpty(dir_00))
				throw new Exception("Bad dir_00");

			if (!Directory.Exists(dir_00))
				throw new Exception("no dir_00");

			string dir_01 = Path.Combine(dir_00, PATH_01);

			if (!Directory.Exists(dir_01))
				throw new Exception("no dir_01");

			string[] dir_02s = Directory.GetDirectories(dir_01)
				.Where(dir => Regex.IsMatch(Path.GetFileName(dir), PATH_02_REGEX))
				.ToArray();

			if (dir_02s.Length == 0)
				throw new Exception("no dir_02s");

			Array.Sort(dir_02s, (a, b) =>
			{
				int[] ver1 = GGPM_Dir02ToVersion(a);
				int[] ver2 = GGPM_Dir02ToVersion(b);

				return SCommon.Comp(ver1, ver2, SCommon.Comp) * -1; // DESC
			});

			foreach (string dir_02 in dir_02s)
			{
				string file_03 = Path.Combine(dir_02, PATH_03);

				if (File.Exists(file_03))
				{
					file_03 = SCommon.MakeFullPath(file_03); // 2bs
					ProcMain.WriteLog("GGPM_file_03: " + file_03); // log
					return file_03;
				}
			}
			throw new Exception("no file_03");
		}

		private static int[] GGPM_Dir02ToVersion(string dir_02)
		{
			string localName = Path.GetFileName(dir_02);
			string[] sVersion = SCommon.Tokenize(localName, SCommon.DECIMAL, true, true);
			int[] version = sVersion.Select(v => int.Parse(v)).ToArray();

			if (version.Length != 3)
				throw new Exception("Bad dir_02");

			return version;
		}
	}
}
