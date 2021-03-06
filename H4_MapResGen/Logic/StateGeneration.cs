﻿using System;
using System.Collections.Generic;
using System.Linq;

using H4_MapResGen.Entities.States;

namespace H4_MapResGen.Logic
{
	public class StateGeneration
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

		public static State GenerateState (int counter, int stateId, StatesContainer importedObjects)
		{	
			if (importedObjects.ImportedDefaults != null)
			{
				ExtractAndUpdateDefaultsToImport (importedObjects.ImportedDefaults);
			}

			int manpower = MainClass.GetRandomNumber (_manpowerMinValue, _manpowerMaxValue);
			string TAG = GetTAG (importedObjects.ImportedTAGs);
			string secondTAG = GetTAG (importedObjects.ImportedTAGs);

			History history = new History ();
			history.Owner = "owner = " + TAG;
			history.VictoryPoints1 = MainClass.GetRandomNumber (0, _vp1MaxValue);
			history.VictoryPoints2 = MainClass.GetRandomNumber (0, _vp2MaxValue);
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
				BuildingMaxLevelFactor = (double)(MainClass.GetRandomNumber (0, 100) / 100), //get as number between 0 and 1
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
			int number = MainClass.GetRandomNumber (0, importedTAGs.Count);
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

			var number = MainClass.GetRandomNumber (0, importedNames.Count);
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
			var enumNumber = MainClass.GetRandomNumber (0, geographyMaxValue);
			var nounNumber = MainClass.GetRandomNumber (0, nouns.Count);
			return nouns [nounNumber] + " " + (Geography)enumNumber;
		}

		public static StateCategories GetStateCategory (int manpower)
		{
			if (manpower < 50000) {
				return (StateCategories)MainClass.GetRandomNumber (0, 1);
			}

			if (manpower >= 50000 && manpower < 250000) {
				return (StateCategories)MainClass.GetRandomNumber (4, 5);
			}

			if (manpower >= 250000 && manpower < 750000) {
				return (StateCategories)MainClass.GetRandomNumber (6, 7);
			}

			if (manpower >= 750000 && manpower < 1000000) {
				return (StateCategories)MainClass.GetRandomNumber (8, 9);
			}

			if (manpower >= 1000000) {
					return (StateCategories)MainClass.GetRandomNumber (10, 11);
			}

			return StateCategories.pastoral; //this shouldn't happen, but just incase.
		}

		public static Dictionary<string,int> PopulateResources ()
		{
			Dictionary<string,int> retVal = new Dictionary<string,int> (); 
			string[] input = { "oil", "aluminium", "rubber", "tungsten", "steel", "chromium" };
			var resources = new List<string> (input);
			foreach (var resource in resources) {
				retVal.Add (resource, MainClass.GetRandomNumber (_resourceLowerBound, _resourceUpperBound));
			}

			return retVal;
		}

		public static Dictionary<string,int> PopulateBuildings ()
		{
			Dictionary<string,int> retVal = new Dictionary<string,int> ();
			string[] input = { "infrastructure", "air_base" }; //, "naval_base", "bunker", "coastal_bunker" };
			var buildings = new List<string> (input);
			foreach (var building in buildings) {
				retVal.Add (building, MainClass.GetRandomNumber (_buildingLowerBound, _buildingMax10));
			}

			retVal.Add ("arms_factory", MainClass.GetRandomNumber (_buildingLowerBound, _buildingMax20));
			retVal.Add ("industrial_complex", MainClass.GetRandomNumber (_buildingLowerBound, _buildingMax20));
			//retVal.Add ("dockyard", MainClass.GetRandomNumber (_buildingLowerBound, _buildingMax20));
			retVal.Add ("anti_air_building", MainClass.GetRandomNumber (_buildingLowerBound, _buildingMax5));
			retVal.Add ("radar_station", MainClass.GetRandomNumber (_buildingLowerBound, _buildingMax3));

			if (_toggleRocketsandNukes) {
				retVal.Add ("nuclear_reactor", MainClass.GetRandomNumber (0,1));
				retVal.Add ("rocket_site", MainClass.GetRandomNumber(0,1));
			}

			return retVal;
		}

		public static string GenerateProvincesString (string vp1provID, string vp2provID, List<string> importedProvinces, Dictionary<int,string> provinceDict)
		{
			string fillerProvID = "ProvinceID";
			string output = " # generationfailed";
			var provinceNumber = MainClass.GetRandomNumber (3, 6);
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
					int randomDictNum = MainClass.GetRandomNumber (0, provinceDict.Count);
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

		public static List<string> SerialiseState (State newState, List<string> importedProvinces)
		{
			string vp1provID = "ProvinceID";
			string vp2provID = "ProvinceID";
			Dictionary<int,string> provinceDict = new Dictionary<int, string> ();

			if (importedProvinces != null) {
				int i = 0;
				foreach (var provinceID in importedProvinces) {
					provinceDict.Add (i, provinceID);
					i++;
				}
				provinceDict.TryGetValue (MainClass.GetRandomNumber (0, provinceDict.Count), out vp1provID);
				provinceDict.TryGetValue (MainClass.GetRandomNumber (0, provinceDict.Count), out vp2provID);			
			}

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
				retVal.Add ("\t\t\t" + vp1provID + " " + newState.History.VictoryPoints1 + " #change if ProvinceID");
				retVal.Add ("\t\t}");
			}
			if (newState.History.VictoryPoints2 != 0) {
				retVal.Add ("\t\tvictory_points = {");
				retVal.Add ("\t\t\t" + vp2provID + " " + newState.History.VictoryPoints2 + " #change if ProvinceID");
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
			if (newState.History.Core2.IsNotNullOrEmpty ()) {
				retVal.Add ("\t\t" + newState.History.Core2);}
			retVal.Add ("\t}");
			retVal.Add ("");
			retVal.Add ("\tprovinces = {");

			string provinces = GenerateProvincesString(vp1provID, vp2provID, importedProvinces, provinceDict);

			retVal.Add ("\t\t" + vp1provID + " " + vp2provID + " " + provinces + " #change these if ProvinceID");
			retVal.Add ("\t}");
			retVal.Add ("}");

			return retVal;
		}
	}
}