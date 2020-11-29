using System;
using H4_MapResGen.Entities.Definitions;

namespace H4_MapResGen.Logic
{
	public class Definitions
	{
		public static void DefinitionsMain ()
		{
 			DefinitionContainer container = new DefinitionContainer ();
			Console.WriteLine ("Input number of provinces requiring definition:");
			var input = Console.ReadLine ();
			int provNumber = 1;
			Int32.TryParse (input, out provNumber);
			container.provNumber = provNumber;

			Console.WriteLine ("Start from 1? (y/n):");
			input = Console.ReadLine ();
			bool startat1 = MainClass.ValidateYesNoInput (input);
			container.provId = 1;

			if (!startat1) {
				int provId = 1;
				Console.WriteLine ("Enter start value:");
				input = Console.ReadLine ();
				Int32.TryParse (input, out provId);
				container.provId = provId;
			}

			Console.WriteLine ("Input upper number of continents:");
			input = Console.ReadLine ();
			int continents = 1;
			Int32.TryParse (input, out continents);
			container.continents = continents;

			Console.WriteLine ("Input lower number of continents:");
			input = Console.ReadLine ();
			int continentsLowerBound = 1;
			Int32.TryParse (input, out continentsLowerBound);
			container.continentsLowerBound = continentsLowerBound;

			int LSLinput = 99;
			Console.WriteLine ("Override random generation of Land, Sea and Lakes? (y/n):");
			input = Console.ReadLine ();
			bool LSLoverride = MainClass.ValidateYesNoInput (input);
			if (LSLoverride) {
				Console.WriteLine ("Land (0) Sea (1) Lakes (2):");
				input = Console.ReadLine ();
				Int32.TryParse (input, out continentsLowerBound);
				container.LSLinput = LSLinput;
			}

			string filename = string.Empty;
			Console.WriteLine ("Save to custom filename? (y/n):");
			input = Console.ReadLine ();
			bool nonDefaultFilename = MainClass.ValidateYesNoInput (input);
			if (nonDefaultFilename) {
				Console.WriteLine ("Enter filename (excluding extension):");
				filename = Console.ReadLine ();
			}

			Console.WriteLine ("Generating definitions...");
			var definitions = DefinitionGeneration.GenerateDefinitions (container);
			Console.WriteLine ("Saving output to local folder.");
			FileManagement.SaveDefinitionstoCsv (definitions, filename);

			Console.WriteLine ("Generation of {0} province definitions, complete", container.provNumber);
			Console.WriteLine ("Press Any Key to Kill.");
			Console.ReadKey ();
		}
	}
}

