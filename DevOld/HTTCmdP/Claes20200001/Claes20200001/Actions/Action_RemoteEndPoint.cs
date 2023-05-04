using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Charlotte.WebServices;
using Charlotte.Commons;

namespace Charlotte.Actions
{
	public static class Action_RemoteEndPoint
	{
		public static void Perform(HTTPServerChannel channel)
		{
			string address = SCommon.Tokenize(channel.Channel.Handler.RemoteEndPoint.ToString(), ":")[0];

			if (100 < address.Length) // rough limit
				throw new Exception("Bad IP-address");

			channel.ResStatus = 200;
			channel.ResHeaderPairs.Add(new string[] { "Content-Type", "text/plain; charset=US-ASCII" });
			channel.ResBody = new byte[][] { Encoding.ASCII.GetBytes(address) };
			channel.ResBodyLength = -1L;
		}
	}
}
