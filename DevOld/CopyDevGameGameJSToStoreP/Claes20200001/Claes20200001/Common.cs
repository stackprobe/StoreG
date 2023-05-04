using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Charlotte
{
	public static class Common
	{
		public static bool ExistsPath(string path)
		{
			return Directory.Exists(path) || File.Exists(path);
		}
	}
}
