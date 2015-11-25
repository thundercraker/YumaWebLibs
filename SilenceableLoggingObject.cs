using System;

namespace YumaWebLib
{
	public class SilenceableLoggingObject
	{
		public static bool BE_QUIET = false;

		public static void Log(string m) {
			if(!BE_QUIET)
				Console.WriteLine (m);
		}
	}
}

