using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Charlotte.WebServices;
using Charlotte.Actions;

namespace Charlotte
{
	public static class ActionServer
	{
		public static bool TryPerform(bool head, string urlPath, HTTPServerChannel channel)
		{
			Action<HTTPServerChannel> a_action = GetAction(urlPath);

			if (a_action == null) // ? アクション無し
				return false;

			if (head)
				throw new Exception("HEAD-リクエストの場合はアクションを実行できません");

			a_action(channel);
			return true;
		}

		/// <summary>
		/// urlPath に対応するアクションを返す。
		/// アクションが無い場合は null を返す。
		/// urlPath -- リクエストの最初の行の3つあるトークンの2つ目からクエリ('?'以降)を取り除いた文字列
		/// -- 例：
		/// ---- /
		/// ---- /aaa
		/// ---- /aaa/
		/// ---- /aaa/bbb
		/// ---- /aaa/bbb.ccc
		/// アクションは channel に応答情報を設定しなければならない。
		/// 設定例：
		/// -- channel.ResStatus = 200;
		/// -- channel.ResHeaderPairs.Add(new string[] { "Content-Type", "text/plain; charset=US-ASCII" });
		/// -- channel.ResBody = new byte[][] { Encoding.ASCII.GetBytes("OK") };
		/// -- channel.ResBodyLength = -1L;
		/// </summary>
		/// <param name="urlPath">URL</param>
		/// <returns>アクション</returns>
		private static Action<HTTPServerChannel> GetAction(string urlPath)
		{
			if (urlPath == "/AccessCounter-barnatsutobi")
				return channel => Action_AccessCounter.Perform(channel, "barnatsutobi");

			if (urlPath == "/AccessCounter-ornithopter")
				return channel => Action_AccessCounter.Perform(channel, "ornithopter");

			if (urlPath == "/AccessCounter-anemoscope")
				return channel => Action_AccessCounter.Perform(channel, "anemoscope");

			if (urlPath == "/API-RemoteEndPoint")
				return channel => Action_RemoteEndPoint.Perform(channel);

			if (urlPath == "/BatchService/" + Common.BatchServiceCredentials)
				return channel => Action_BatchService.Perform(channel);

			if (urlPath.StartsWith("/API-Download/"))
				return channel => Action_Download.Perform(channel);

			return null;
		}
	}
}
