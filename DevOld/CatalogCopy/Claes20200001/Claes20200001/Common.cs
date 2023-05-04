using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Charlotte.Commons;

namespace Charlotte
{
	public static class Common
	{
		public static bool IsFairTimeStamp(long timeStamp)
		{
			return SCommon.TimeStampToSec.ToTimeStamp(SCommon.TimeStampToSec.ToSec(timeStamp)) == timeStamp;
		}
	}
}
