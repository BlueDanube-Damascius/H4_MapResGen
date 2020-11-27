using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

using H4_MapResGen.Entities;

namespace H4_MapResGen.Logic
{
	public class FileManagement
	{
		public static char[] seperators = {',',';','-'};
		public static string GetImportFile (string search)
		{
			var importFile = Directory.GetFiles (Environment.CurrentDirectory, search);	
			if (importFile.Any (x => x.IsNotNullOrEmpty ())) {
				return importFile.First ();
			}
			return "Not Found";
		}

		public static List<string> ImportText (string filename)
		{
			var import = new List<string> ();
			using (StreamReader sr = new StreamReader (Path.Combine (Environment.CurrentDirectory, filename))) {
				import = sr.ReadToEnd ().Split (seperators).ToList ();
			}

			return SanitiseImport (import);
		}

		public static List<string> SanitiseImport(List<string> import)
		{
			var sanitisedTexts = new List<string> ();
			foreach (var str in import) {
				var sanitisedText = str.Trim ();
				if (sanitisedText.IsNotNullOrEmpty ()) {
					sanitisedTexts.Add (sanitisedText);
				}
			}
			return sanitisedTexts;
		}

		public static void SaveStatetoTextFile (State newState)
		{
			var filename = Generation.GenerateFileName (newState.StateId, newState.StateName);
			var payload = SerialiseState (newState);
			using (StreamWriter sw = new StreamWriter (Path.Combine (Environment.CurrentDirectory, filename))) {
				foreach (var line in payload) {
					sw.WriteLine (line);
				}
			}
		}

		public static List<string> SerialiseState (State newState)
		{
			var retVal = new List<string> ();
			retVal.Add ("");
			retVal.Add ("state = {");
			retVal.Add ("\tid = " + newState.StateId);
			retVal.Add ("\tname = " + newState.StateName);
			retVal.Add ("\tmanpower = " + newState.Manpower);
			retVal.Add ("");
			retVal.Add ("\tstate_category = " + newState.StateCategory);
			retVal.Add ("");
			retVal.Add ("\tresources = {");
			foreach (var pair in newState.Resources) {
				if (pair.Value != 0) {
					retVal.Add ("\t\t" + pair.Key + "=" + pair.Value);
				}
			}
			retVal.Add ("\t}");
			retVal.Add ("");
			retVal.Add ("\thistory = {");
			retVal.Add ("\t\t" + newState.History.Owner);
			retVal.Add ("");
			if (newState.History.VictoryPoints1 != 0) {
				retVal.Add ("\t\tvictory_points = {");
				retVal.Add ("\t\t\tProvinceID " + newState.History.VictoryPoints1 + "#change ProvinceID");
				retVal.Add ("\t\t}");
			}
			if (newState.History.VictoryPoints2 != 0) {
				retVal.Add ("\t\tvictory_points = {");
				retVal.Add ("\t\t\tProvinceID " + newState.History.VictoryPoints2 + "#change ProvinceID");
				retVal.Add ("\t\t}");
			}
			retVal.Add ("");
			retVal.Add ("\t\tbuildings = {");
			foreach (var pair in newState.History.Buildings) { 
				if (pair.Value != 0) {
					retVal.Add ("\t\t\t" + pair.Key + "=" + pair.Value);
				}
			}
			retVal.Add ("\t\t}");
			retVal.Add ("");
			retVal.Add ("\t\t" + newState.History.Core);
			retVal.Add ("\t}");
			retVal.Add ("");
			retVal.Add ("\tprovinces = {");
			retVal.Add ("\tProvinceID ProvinceID ProvinceID ProvinceID ProvinceID ProvinceID ProvinceID ProvinceID ProvinceID #change these");
			retVal.Add ("\t}");
			retVal.Add ("}");

			return retVal;
		}
	}
}