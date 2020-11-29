//version 0.1 written by Damascius aka BlueDanube
//27-Nov-2020. 
//GNU GPLv3 License.

using System;
using System.Collections.Generic;
using System.IO;

using H4_MapResGen.Entities;
using H4_MapResGen.Logic;

namespace H4_MapResGen
{
	public class MainClass
	{
		private static readonly string Version = "0.1.3";
		private static readonly DateTime VersionDate = new DateTime (2020, 11, 28);
		private static readonly Random getrandom = new Random ();

		public static int GetRandomNumber (int min, int max)
		{
			lock (getrandom) { // synchronize
				return getrandom.Next (min, max);
			}
		}
		public static void Main (string[] args)
		{
			Console.WriteLine ("Welcome to HoI4 State Generator, written by Damascius aka BlueDanube");
			Console.WriteLine ("Version number: {0} - {1}", Version, VersionDate);
			Console.WriteLine ("Select operation:");
			Console.WriteLine ("S - Generate new (S)tates");
			Console.WriteLine ("D - Generate new rows for (D)efinitions.csv");
			bool operationSelected = false;
			while (!operationSelected) {
				var input = Console.ReadLine ();
				if (input == "S" || input == "s") {
					States.StatesMain ();
					operationSelected = true;
				}
				if (input == "D" || input == "d") {
					Definitions.DefinitionsMain ();
					operationSelected = true;
				}
				if (!operationSelected) {
					Console.WriteLine ("Bad input, please try again.");
				}
			}
		}

		public static bool ValidateYesNoInput (string input)
		{
			switch (input) {
			case "y":
			case "Y":
				return true;
			case "n":
			case "N":
				return false;
			default:
				Console.WriteLine ("Bad Input, assuming NO");
				return false;
			}

		}
	}
}