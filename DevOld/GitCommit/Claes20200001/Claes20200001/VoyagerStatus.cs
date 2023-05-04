using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Charlotte.Commons;
using Charlotte.Utilities;

namespace Charlotte
{
	public class VoyagerStatus
	{
		private const string NASA_DISTANCE_DATA_URL = "https://voyager.jpl.nasa.gov/assets/javascripts/distance_data.js";

		private static readonly long EPOCH_TIME = SCommon.TimeStampToSec.ToSec(19700101000000);

		public class DistanceInfo
		{
			public long Epoch;
			public double Kilometer;

			public DistanceInfo(long epoch, double kilometer)
			{
				this.Epoch = epoch;
				this.Kilometer = kilometer;
			}
		}

		public class DistancePairInfo
		{
			public DistanceInfo[] Pair = new DistanceInfo[2]; // { Today , Yesterday }

			public double GetKilometerPerSecond() // ret: Velocity
			{
				return (this.Pair[0].Kilometer - this.Pair[1].Kilometer) / (this.Pair[0].Epoch - this.Pair[1].Epoch);
			}

			private double GetKilometer(long epoch) // ret: Distance from Earth or Sun
			{
				return this.Pair[1].Kilometer + (epoch - this.Pair[1].Epoch) * this.GetKilometerPerSecond();
			}

			public double GetKilometer(DateTime dateTime)
			{
				return this.GetKilometer(SCommon.TimeStampToSec.ToSec(dateTime.ToUniversalTime()) - EPOCH_TIME);
			}
		}

		public DistancePairInfo DistanceVoyager1Earth = new DistancePairInfo();
		public DistancePairInfo DistanceVoyager2Earth = new DistancePairInfo();
		public DistancePairInfo DistanceVoyager1Sun = new DistancePairInfo();
		public DistancePairInfo DistanceVoyager2Sun = new DistancePairInfo();

		public VoyagerStatus()
		{
			HTTPClient hc = new HTTPClient(NASA_DISTANCE_DATA_URL);

			hc.ConnectTimeoutMillis = 10000; // 10 sec
			hc.TimeoutMillis = 15000; // 15 sec
			hc.IdleTimeoutMillis = 5000; // 5 sec
			hc.ResBodySizeMax = 1000000; // 1 MB

			using (WorkingDir wd = new WorkingDir())
			{
				hc.ResFile = wd.MakePath();
				hc.Get();

				Dictionary<string, string> values = SCommon.CreateDictionary<string>();

				foreach (string line in File.ReadAllLines(hc.ResFile, Encoding.ASCII).Where(v => v != ""))
				{
					string[] tokens = SCommon.Tokenize(line, " ");

					string name = tokens[1];
					string value = tokens[3].Replace(";", "");

					values.Add(name, value);
				}

				long epoch_0 = long.Parse(values["epoch_0"]);
				long epoch_1 = long.Parse(values["epoch_1"]);

				this.DistanceVoyager1Earth.Pair[0] = new DistanceInfo(epoch_0, double.Parse(values["dist_0_v1"]));
				this.DistanceVoyager1Earth.Pair[1] = new DistanceInfo(epoch_1, double.Parse(values["dist_1_v1"]));

				this.DistanceVoyager2Earth.Pair[0] = new DistanceInfo(epoch_0, double.Parse(values["dist_0_v2"]));
				this.DistanceVoyager2Earth.Pair[1] = new DistanceInfo(epoch_1, double.Parse(values["dist_1_v2"]));

				this.DistanceVoyager1Sun.Pair[0] = new DistanceInfo(epoch_0, double.Parse(values["dist_0_v1s"]));
				this.DistanceVoyager1Sun.Pair[1] = new DistanceInfo(epoch_1, double.Parse(values["dist_1_v1s"]));

				this.DistanceVoyager2Sun.Pair[0] = new DistanceInfo(epoch_0, double.Parse(values["dist_0_v2s"]));
				this.DistanceVoyager2Sun.Pair[1] = new DistanceInfo(epoch_1, double.Parse(values["dist_1_v2s"]));
			}

			// 取得した値の簡単なチェック
			{
				DateTime now = DateTime.Now;

				// --

				// 太陽からは常に遠ざかるはず。

				if (this.DistanceVoyager1Sun.GetKilometerPerSecond() < 0.0) // v1
					throw new Exception("v1s Bad Velocity");

				if (this.DistanceVoyager2Sun.GetKilometerPerSecond() < 0.0) // v2
					throw new Exception("v2s Bad Velocity");

				// --

				// 太陽との相対速度と地球との相対速度の差が地球の公転速度を超えることはないはず。
				// 地球の公転速度 == 秒速29.78キロメートル

				const double VELOCITY_DIFF_MAX = 40.0; // == 地球の公転速度 + マージン // 誤差がどの程度あるか判然としなかったのでマージン多め(約1/3) // 注意：キロメートル毎秒

				// memo: 両探査機共に地球の公転速度より遅いので、地球との相対速度がマイナスになる(地球に近づいてくる)ことがある。

				if (VELOCITY_DIFF_MAX < Math.Abs(this.DistanceVoyager1Sun.GetKilometerPerSecond() - this.DistanceVoyager1Earth.GetKilometerPerSecond())) // v1
					throw new Exception("v1e Bad Velocity");

				if (VELOCITY_DIFF_MAX < Math.Abs(this.DistanceVoyager2Sun.GetKilometerPerSecond() - this.DistanceVoyager2Earth.GetKilometerPerSecond())) // v2
					throw new Exception("v2e Bad Velocity");

				// --

				// 2022年8月28日の時点で v1s == 235.22億km , v2s == 195.27億km に達しているので今はそれよりも遠いはず。

				if (this.DistanceVoyager1Sun.GetKilometer(now) < 23522000000.0) // v1
					throw new Exception("v1s Bad Distance");

				if (this.DistanceVoyager2Sun.GetKilometer(now) < 19527000000.0) // v2
					throw new Exception("v2s Bad Distance");

				// --

				// 太陽との相対距離と地球との相対距離の差が地球の公転半径を超えることはないはず。
				// 地球の公転半径 == 1.495978707億キロメートル

				const double DISTANCE_DIFF_MAX = 200000000.0; // == 地球の公転半径 + マージン // 誤差がどの程度あるか判然としなかったのでマージン多め(約1/3)

				if (DISTANCE_DIFF_MAX < Math.Abs(this.DistanceVoyager1Sun.GetKilometer(now) - this.DistanceVoyager1Earth.GetKilometer(now))) // v1
					throw new Exception("v1e Bad Distance");

				if (DISTANCE_DIFF_MAX < Math.Abs(this.DistanceVoyager2Sun.GetKilometer(now) - this.DistanceVoyager2Earth.GetKilometer(now))) // v2
					throw new Exception("v2e Bad Distance");

				// --
			}
		}
	}
}
