using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.IO;
using Charlotte.Commons;

namespace Charlotte
{
	public class VoyagerSunStatusFile
	{
		public long TimeStamp;
		public double V1S_Kilometer;
		public double V2S_Kilometer;
		public double V1S_KilometerPerSecond;
		public double V2S_KilometerPerSecond;

		public VoyagerSunStatusFile(string saveDataFile)
		{
			if (string.IsNullOrEmpty(saveDataFile))
				throw new Exception("Bad saveDataFile");

			LoadFile(saveDataFile);

			VoyagerStatus vv = null;
			DateTime now = DateTime.Now;
			long timeDiff = SCommon.TimeStampToSec.ToSec(now) - SCommon.TimeStampToSec.ToSec(TimeStamp);

			if (timeDiff < 3600)
			{
				ProcMain.WriteLog("**** v1-2st取得中止：前回の取得から1時間以内 " + timeDiff);
			}
			else
			{
				try
				{
					vv = new VoyagerStatus();
				}
				catch (Exception ex)
				{
					vv = null; // 2bs

					ProcMain.WriteLog("**** v1-2st取得失敗：" + ex);
					ProcMain.WriteLog("**** v1-2st取得失敗しましたが処理は継続します。");
				}

				// 再設定 -- 通信に数秒～数十秒掛かったかもしれない。
				now = DateTime.Now;
				timeDiff = SCommon.TimeStampToSec.ToSec(now) - SCommon.TimeStampToSec.ToSec(TimeStamp);
			}

			TimeStamp = SCommon.TimeStampToSec.ToTimeStamp(now);

			if (vv != null)
			{
				V1S_Kilometer = vv.DistanceVoyager1Sun.GetKilometer(now);
				V2S_Kilometer = vv.DistanceVoyager2Sun.GetKilometer(now);
				V1S_KilometerPerSecond = vv.DistanceVoyager1Sun.GetKilometerPerSecond();
				V2S_KilometerPerSecond = vv.DistanceVoyager2Sun.GetKilometerPerSecond();

				SaveFile(saveDataFile);
			}
			else
			{
				V1S_Kilometer += V1S_KilometerPerSecond * timeDiff;
				V2S_Kilometer += V2S_KilometerPerSecond * timeDiff;
			}
		}

		private void LoadFile(string saveDataFile)
		{
			// Init -- 既知の値 @ 2022.11.11
			{
				TimeStamp = 20221111220136;
				V1S_Kilometer = 23657707853.6549;
				V2S_Kilometer = 19711030588.1137;
				V1S_KilometerPerSecond = 16.8917322407387;
				V2S_KilometerPerSecond = 15.0294261643622;
			}

			if (File.Exists(saveDataFile))
			{
				string[] lines = File.ReadAllLines(saveDataFile, Encoding.ASCII);
				int c = 0;

				TimeStamp = long.Parse(lines[c++]);
				V1S_Kilometer = double.Parse(lines[c++]);
				V2S_Kilometer = double.Parse(lines[c++]);
				V1S_KilometerPerSecond = double.Parse(lines[c++]);
				V2S_KilometerPerSecond = double.Parse(lines[c++]);
			}
		}

		private void SaveFile(string saveDataFile)
		{
			string[] lines = new string[]
			{
				TimeStamp.ToString(),
				V1S_Kilometer.ToString("F20"),
				V2S_Kilometer.ToString("F20"),
				V1S_KilometerPerSecond.ToString("F20"),
				V2S_KilometerPerSecond.ToString("F20"),
			};

			File.WriteAllLines(saveDataFile, lines, Encoding.ASCII);
		}
	}
}
