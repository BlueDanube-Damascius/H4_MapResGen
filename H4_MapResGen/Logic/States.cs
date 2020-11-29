using System;
using System.Collections.Generic;

using H4_MapResGen.Entities.States;

namespace H4_MapResGen.Logic
{
	public class States
	{
		public static void StatesMain ()
		{
			Console.WriteLine ("Input number of states requiring generation:");
			var input = Console.ReadLine ();
			int stateNumber;
			Int32.TryParse (input, out stateNumber);

			Console.WriteLine ("Start from 1? (y/n):");
			input = Console.ReadLine ();
			bool startat1 = MainClass.ValidateYesNoInput (input);
			int stateId = 0;

			if (!startat1) {
				Console.WriteLine ("Enter start value:");
				input = Console.ReadLine ();
				Int32.TryParse (input, out stateId);
			}

			StatesContainer container = new StatesContainer ();
			List<string> importStrings = new List<string> { "State Name", "TAG", "Province ID", "New Generation Defaults" };
			foreach (var importString in importStrings) {
				Console.WriteLine ("Import {0} list from local folder? (y/n)", importString);
				input = Console.ReadLine ();
				bool import = MainClass.ValidateYesNoInput (input);

				if (import) {
					bool correctFile = false;
					string filename = "foo";
					while (!correctFile) {
						Console.WriteLine ("Please input filename (CasE-SenSiTiVe) (including extension)");
						var search = Console.ReadLine ();
						filename = FileManagement.GetImportFile (search);
						Console.WriteLine ("Is the file: {0} ? (y/n)", filename);
						input = Console.ReadLine ();
						correctFile = MainClass.ValidateYesNoInput (input);
					}

					if (correctFile) {
						switch (importString) {
						case "State Name":
							container.ImportedNames = FileManagement.ImportText (filename);
							break;
						case "TAG":
							container.ImportedTAGs = FileManagement.ImportText (filename);
							break;
						case "Province ID":
							container.ImportedProvinces = FileManagement.ImportText (filename);
							break;
						case "New Generation Defaults":
							container.ImportedDefaults = FileManagement.ImportText (filename);
							break;
						}
					}
				}
			}

			Console.WriteLine ("Do you want to generate multiple core owners for one state (y/n)");
			Console.WriteLine ("WARNING: second tag is randomly assigned from the list imported - advised you import a limited selection of TAGs if you do this.");
			Console.WriteLine ("If you didn't import a TAG list, or the random assignment of TAG is the same twice, you'll only get one row, no matter what you pick here.");
			input = Console.ReadLine ();
			container.TwoCores = MainClass.ValidateYesNoInput (input);

			int counter = 0;
			while (counter < stateNumber) {
				Console.WriteLine ("Generating states...");
				State newState = StateGeneration.GenerateState (counter, stateId, container);
				Console.WriteLine ("State: {0} generated", newState.StateName);
				Console.WriteLine ("Saving output to local folder.");
				FileManagement.SaveStatetoTextFile (newState, container.ImportedProvinces);
				if (stateId != 0) {
					stateId++;
				}
				counter++;
			}
			Console.WriteLine ("Generation of {0} states, complete", counter);
			Console.WriteLine ("Press Any Key to Kill.");
			Console.ReadKey ();
		}
	}
}