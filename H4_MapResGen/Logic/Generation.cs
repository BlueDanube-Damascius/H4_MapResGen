using System;
using System.Collections.Generic;
using System.Linq;

using H4_MapResGen.Entities;

namespace H4_MapResGen.Logic
{
	public class Generation
	{
		private const int geographyMaxValue = 21;
		//enum currently contains 22 items.

		private static int _manpowerMinValue = 10000;
		private static int _manpowerMaxValue = 3000000;
		private static int _vp1MaxValue = 50;
		private static int _vp2MaxValue = 20;
		private static int _resourceLowerBound = 0;
		private static int _resourceUpperBound = 50;
		private static int _buildingLowerBound = 0;
		private static int _buildingMax3 = 3;
		private static int _buildingMax5 = 5;
		private static int _buildingMax10 = 10;
		private static int _buildingMax20 = 20;
		private static bool _toggleRocketsandNukes = false;

		private static readonly Random getrandom = new Random ();

		public static int GetRandomNumber (int min, int max)
		{
			lock (getrandom) { // synchronize
				return getrandom.Next (min, max);
			}
		}

		public static State GenerateState (int counter, int stateId, ImportContainer importedObjects)
		{
			if (importedObjects.ImportedDefaults != null)
			{
				ExtractAndUpdateDefaultsToImport (importedObjects.ImportedDefaults);
			}

			int manpower = GetRandomNumber (_manpowerMinValue, _manpowerMaxValue);
			string TAG = GetTAG (importedObjects.ImportedTAGs);
			string secondTAG = GetTAG (importedObjects.ImportedTAGs);

			History history = new History ();
			history.Owner = "owner = " + TAG;
			history.VictoryPoints1 = GetRandomNumber (0, _vp1MaxValue);
			history.VictoryPoints2 = GetRandomNumber (0, _vp2MaxValue);
			history.Core = "add_core_of = " + TAG;
			if (TAG != secondTAG && importedObjects.TwoCores) {
				history.Core2 = "add_core_of = " + secondTAG;
			}
			history.Buildings = PopulateBuildings ();

			return new State {
				StateId = GetStateId (counter, stateId),
				StateName = GetStateName (importedObjects.ImportedNames),
				Manpower = manpower,
				StateCategory = GetStateCategory (manpower),
				BuildingMaxLevelFactor = (double)(GetRandomNumber (0, 100) / 100), //get as number between 0 and 1
				Resources = PopulateResources (),
				History = history
			};
		}

		public static void ExtractAndUpdateDefaultsToImport (List<string> importedDefaults)
		{
			const int max = 12;
			int i = 0;
			int[] Out = new int [max];

			while (i < max) {
				Int32.TryParse (importedDefaults [i], out Out [i]);
				if (Out [i] >= 0) {
					if (i < 5) {
						switch (i) {
						case 0:
							_manpowerMinValue = Out [i];
							break;
						case 1:
							_manpowerMaxValue = Out [i];
							break;
						case 2:
							_vp1MaxValue = Out [i];
							break;
						case 3:
							_vp2MaxValue = Out [i];
							break;
						case 4:
							_resourceLowerBound = Out [i];
							break;
						case 5:
							_resourceUpperBound = Out [i];
							break;
						}
					}
					if ((i == 6 || i == 7) && Out[i] <= 3) {
						switch (i) {
						case 6:
							_buildingLowerBound = Out [i];
							break;
						case 7:
							_buildingMax3 = Out [i];
							break;
						}
					}
					if (i == 8 && Out [i] <= 5) {
						_buildingMax5 = Out [i];
					}
					if (i == 9 && Out [i] <= 10) {
						_buildingMax5 = Out [i];
					}
					if (i == 10 && Out [i] <= 20) {
						_buildingMax5 = Out [i];
					}
					if (i == 11 && Out [i] == 1) {
						_toggleRocketsandNukes = true;
					}
					i++;
				}
			}
		}

		public static string GetTAG (List<string> importedTAGs)
		{
			if (importedTAGs == null) {
				return "TAG #change this manually";
			}
			int number = GetRandomNumber (0, importedTAGs.Count);
			return importedTAGs [number];
		}

		public static int GetStateId (int counter, int stateId)
		{
			if (stateId != 0) {
				return stateId;
			} else {
				return counter + 1;
			}
		}

		public static string GetStateName (List<string> importedNames)
		{
			string retval;
			if (importedNames == null) {
				return GenerateStateName ();
			}

			var number = GetRandomNumber (0, importedNames.Count);
			retval = importedNames [number];
			importedNames.Remove (retval);
			return retval;
		}

		public static string GenerateStateName ()
		{
			string[] input = {
				"Watkins",
				"Atreides",
				"Harkonnen",
				"Harlan",
				"Davis",
				"Said",
				"Lenin",
				"Shield",
				"Needle",
				"Fanon",
				"Gilroy",
				"Mieville",
				"Watts",
				"Horne",
				"Eilenberger",
				"van Reybrouch",
				"Aitmatov",
				"Nkrumah",
				"Ayers",
				"Al-Mansoor",
				"Liang"
			};
			var nouns = new List<string> (input);
			var enumNumber = GetRandomNumber (0, geographyMaxValue);
			var nounNumber = GetRandomNumber (0, nouns.Count);
			return nouns [nounNumber] + " " + (Geography)enumNumber;
		}

		public static StateCategories GetStateCategory (int manpower)
		{
			if (manpower < 50000) {
				var number = GetRandomNumber (0, 1);
				return (StateCategories)number;
			}

			if (manpower >= 50000 && manpower < 250000) {
				var number = GetRandomNumber (4, 5);
				return (StateCategories)number;
			}


			if (manpower >= 250000 && manpower < 750000) {
				var number = GetRandomNumber (6, 7);
				return (StateCategories)number;
			}

			if (manpower >= 750000 && manpower < 1000000) {
				var number = GetRandomNumber (8, 9);
				return (StateCategories)number;
			}

			if (manpower >= 1000000) {
				var number = GetRandomNumber (10, 11);
				return (StateCategories)number;
			}

			return StateCategories.pastoral; //this shouldn't happen, but just incase.
		}

		public static Dictionary<string,int> PopulateResources ()
		{
			Dictionary<string,int> retVal = new Dictionary<string,int> (); 
			string[] input = { "oil", "aluminium", "rubber", "tungsten", "steel", "chromium" };
			var resources = new List<string> (input);
			foreach (var resource in resources) {
				retVal.Add (resource, GetRandomNumber (_resourceLowerBound, _resourceUpperBound));
			}

			return retVal;
		}

		public static Dictionary<string,int> PopulateBuildings ()
		{
			Dictionary<string,int> retVal = new Dictionary<string,int> ();
			string[] input = { "infrastructure", "air_base", "naval_base", "bunker", "coastal_bunker" };
			var buildings = new List<string> (input);
			foreach (var building in buildings) {
				retVal.Add (building, GetRandomNumber (_buildingLowerBound, _buildingMax10));
			}

			retVal.Add ("arms_factory", GetRandomNumber (_buildingLowerBound, _buildingMax20));
			retVal.Add ("industrial_complex", GetRandomNumber (_buildingLowerBound, _buildingMax20));
			retVal.Add ("dockyard", GetRandomNumber (_buildingLowerBound, _buildingMax20));
			retVal.Add ("anti_air_building", GetRandomNumber (_buildingLowerBound, _buildingMax5));
			retVal.Add ("radar_station", GetRandomNumber (_buildingLowerBound, _buildingMax3));

			if (_toggleRocketsandNukes) {
				retVal.Add ("nuclear_reactor", GetRandomNumber (0,1));
				retVal.Add ("rocket_site", GetRandomNumber(0,1));
			}

			return retVal;
		}

		public static string GenerateProvincesString (string vp1provID, string vp2provID, List<string> importedProvinces, Dictionary<int,string> provinceDict)
		{
			string fillerProvID = "ProvinceID";
			string output = " # generationfailed";
			var provinceNumber = GetRandomNumber (3, 6);
			List<string> input = new List<string> ();
			input.Add (vp1provID);
			input.Add (vp2provID);

			if (importedProvinces == null) {
				while (input.Count < provinceNumber) {
					input.Add (fillerProvID);
				}
				output = string.Join (" ", input);
			} else if (importedProvinces.Any()) {
				int j = 2; //start at 2 because the first 2 are already provided and we want to have max of 6
				while (j < provinceNumber) {
					int randomDictNum = Generation.GetRandomNumber (0, provinceDict.Count);
					var provinceToAdd = provinceDict [randomDictNum];
					if (provinceToAdd != (vp1provID) || provinceToAdd != (vp2provID)) {
						input.Add (provinceDict [randomDictNum]);
						j++;
					}
				} 
				output = string.Join (" ", input);
			}
			return output;
		}

		public static string GenerateFileName (int stateId, string stateName)
		{
			return stateId + " " + stateName + ".txt";
		}
	}
}