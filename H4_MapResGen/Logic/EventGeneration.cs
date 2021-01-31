using System;
using System.Collections.Generic;
using System.Linq;

using H4_MapResGen.Entities.Events; 

namespace H4_MapResGen.Logic
{
	public class EventGeneration
	{
		public static List<string> EventSerialise(List<EventContainer> events, string _namespace)
		{
			var retVal = new List<string> ();
			retVal.Add (string.Format("add_namespace =" + "\"" + "{0}"+ "\"",_namespace));
			retVal.Add ("");
			foreach (var eve in events) {
				retVal.Add ("\tcountry_event = {");
				retVal.Add ("\t\tid = " + "\"" + eve.id + "\"");
				retVal.Add ("\t\ttitle = " + "\"" + eve.title + "\"");
				retVal.Add ("\t\tdesc = " + "\"" + eve.desc+ "\"");
				retVal.Add ("\t\tis_triggered_only = yes");
				retVal.Add ("\t\tfire_only_once = yes");
				retVal.Add ("\t\t");
				foreach (var option in eve.options) {
					retVal.Add ("\t\t\toption = {");
					retVal.Add (string.Format("\t\t\tname = "+ "\""+"{0}"+ "\"",option));
					retVal.Add ("\t\t\t#put your shit here");
					retVal.Add ("\t\t\t}");
				}
				retVal.Add("\t}");
				retVal.Add("");
			}
			return retVal;
		}
	}
}