using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.IO;
using Charlotte.Commons;
using Charlotte.WebServices;

namespace Charlotte.Actions
{
	public static class Action_AccessCounter
	{
		private class AccessHistoryInfo
		{
			public string Group;
			public string ClientIP;
			public DateTime AccessedTime;
		}

		private static List<AccessHistoryInfo> AccessHistoryList = new List<AccessHistoryInfo>();

		private class AccessCounterInfo
		{
			public string Group;
			public long Counter;
		}

		public static void Perform(HTTPServerChannel channel, string group)
		{
			string resText;
			try
			{
				// ---- 引数チェック

				if (channel == null)
					throw new ArgumentNullException("channel is null");

				if (group == null)
					throw new ArgumentNullException("group is null");

				group = SCommon.ToJString(group, false, false, false, false); // 2bs

				if (!Regex.IsMatch(group, "^[-0-9A-Za-z]+$"))
					throw new Exception("Bad group: " + group);

				// ----

				if (channel.Method != "POST")
					throw new Exception("channel.Method is not POST");

				string clientIPPort = channel.Channel.Handler.RemoteEndPoint.ToString();
				string clientIP = SCommon.Tokenize(clientIPPort, ":")[0];

				clientIP = SCommon.ToJString(clientIP, false, false, false, false); // 2bs

				if (!Regex.IsMatch(clientIP, "^[0-9]{1,3}\\.[0-9]{1,3}\\.[0-9]{1,3}\\.[0-9]{1,3}$"))
					throw new Exception("Bad clientIP: " + clientIP);

				DateTime now = DateTime.Now;

				AccessHistoryList.RemoveAll(accessHistory => 120 <= (now - accessHistory.AccessedTime).TotalSeconds); // 2分以上経過 -> アクセス履歴から除去

				while (1000 < AccessHistoryList.Count) // rough limit // 多すぎる -> 古いアクセス履歴を除去
					AccessHistoryList.RemoveAt(0);

				int accessHistoryIndex = SCommon.IndexOf(AccessHistoryList, accessHistory =>
					accessHistory.Group == group &&
					accessHistory.ClientIP == clientIP
					);

				List<AccessCounterInfo> accessCounterList = File.ReadAllLines(ProgramDataFolder.I.GetAccessCounterFile(), Encoding.UTF8)
					.Where(line => line != "")
					.Select(line =>
					{
						string[] tokens = SCommon.Tokenize(line, "=");

						if (tokens.Length != 2)
							throw new Exception();

						return new AccessCounterInfo()
						{
							Group = tokens[0],
							Counter = long.Parse(tokens[1]),
						};
					})
					.ToList();

				int accessCounterIndex = SCommon.IndexOf(accessCounterList, accessCounter =>
					accessCounter.Group == group
					);

				if (accessCounterIndex == -1) // ? カウンター無し -> 作成する。
				{
					accessCounterList.Add(new AccessCounterInfo()
					{
						Group = group,
						Counter = 0L,
					});

					accessCounterIndex = accessCounterList.Count - 1;
				}

				if (accessHistoryIndex != -1) // ? アクセス履歴に有り
				{
					// noop
				}
				else // ? アクセス履歴に無し
				{
					AccessHistoryList.Add(new AccessHistoryInfo()
					{
						Group = group,
						ClientIP = clientIP,
						AccessedTime = now,
					});

					accessCounterList[accessCounterIndex].Counter++;
				}

				File.WriteAllLines(
					ProgramDataFolder.I.GetAccessCounterFile(),
					accessCounterList
						.Select(accessCounter => accessCounter.Group + "=" + accessCounter.Counter)
						.ToArray(),
					Encoding.UTF8
					);

				resText = accessCounterList[accessCounterIndex].Counter.ToString();
			}
			catch (Exception ex)
			{
				resText = ex.ToString();
			}

			channel.ResStatus = 200;
			channel.ResHeaderPairs.Add(new string[] { "Content-Type", "text/plain; charset=UTF-8" });
			channel.ResBody = new byte[][] { Encoding.UTF8.GetBytes(resText) };
			channel.ResBodyLength = -1L;
		}
	}
}
