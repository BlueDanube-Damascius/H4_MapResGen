using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

using H4_MapResGen.Entities.Definitions;
using H4_MapResGen.Entities.States;

namespace H4_MapResGen.Logic
{
	public class FileManagement
	{
		public static char[] seperators = { ',', ';', '-', '\n' };

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

		public static List<string> SanitiseImport (List<string> import)
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

		public static void SaveStatetoTextFile (State newState, List<string> importedProvinces)
		{
			var filename = StateGeneration.GenerateFileName (newState.StateId, newState.StateName);
			var payload = StateGeneration.SerialiseState (newState, importedProvinces);
			using (StreamWriter sw = new StreamWriter (Path.Combine (Environment.CurrentDirectory, filename))) {
				foreach (var line in payload) {
					sw.WriteLine (line);
				}
			}
		}

		public static void SaveDefinitionstoCsv(List<ProvinceDefinition> definitions, string filename)
		{
			filename = (filename != string.Empty ? filename + ".csv" : "H4_Gen_Definitions.csv");
			var payload = DefinitionGeneration.SerialisedObjects (definitions);
			using (StreamWriter sw = new StreamWriter (Path.Combine (Environment.CurrentDirectory, filename))) {
				foreach (var line in payload) {
					sw.WriteLine (line);
				}
			}
		}

		public static void SaveEventsToFile(List<string> events, string _namespace)
		{
			var filename = _namespace + ".txt";
			using (StreamWriter sw = new StreamWriter (Path.Combine (Environment.CurrentDirectory, filename))) {
				foreach (var line in events) {
					sw.WriteLine (line);
				}
			}
		}
		public static void SaveTechsToFile(List<string> input)
		{
			var filename = string.Format("techlocalisationgenerated{0}.txt", DateTime.Now.ToFileTime());
			using (StreamWriter sw = new StreamWriter(Path.Combine(Environment.CurrentDirectory, filename)))
			{
				foreach (var line in input)
				{
					sw.WriteLine(line);
				}
			}
		}
	}
}