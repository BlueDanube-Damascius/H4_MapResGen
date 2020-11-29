using System;
using System.Linq;

namespace H4_MapResGen.Logic
{
	public static class Utils
	{
		public static bool IsNotNullOrEmpty(this string s)
		{
			return s != null && s != string.Empty;
		}
	}
}