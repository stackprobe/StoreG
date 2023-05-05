using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using Charlotte.Drawings;

namespace Charlotte
{
	public static class GameConfig
	{
		public static string GameTitle = "Gattonero-2020-00-01";

		public static I2Size ScreenSize = new I2Size(960, 540);

		public static double DefaultMusicVolume = 0.45;
		public static double DefaultSEVolume = 0.45;

		public static string[] FontFileResPaths = new string[]
		{
			//@"General\Dummy.ttf",
		};

		public static Color LibbonBackColor = Color.DarkSlateGray;
		public static Color LibbonForeColor = Color.White;
	}
}
