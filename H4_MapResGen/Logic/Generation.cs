using System;
using System.Collections.Generic;

using H4_MapResGen.Entities;

namespace H4_MapResGen.Logic
{
	public class Generation
	{
		private const int geographyMaxValue = 19; //enum currently contains 20 items.

		private static readonly int manpowerMinValue = 10000;
		private static readonly int manpowerMaxValue = 3000000;
		private static readonly int vp1MaxValue = 50;
		private static readonly int vp2MaxValue = 20;
		private static readonly int buildingLowerBound = 0;
		private static readonly int buildingMax3 = 3;
		private static readonly int buildingMax5 = 5;
		private static readonly int buildingMax10 = 10;
		private static readonly int buildingMax20 = 20;

		private static readonly Random getrandom = new Random();

		Generation()
		{
			
		}
		public static int GetRandomNumber(int min, int max)
		{
			lock(getrandom) // synchronize
			{
				return getrandom.Next(min, max);
			}
		}

		public static State GenerateState(int counter, int stateId, ImportContainer importedObjects)
		{
			int manpower = GetRandomNumber(manpowerMinValue, manpowerMaxValue);
			string TAG = "TAG";
			string secondTAG = "TAG";

			History history = new History ();
			history.Owner =  "owner = " + TAG;
			history.VictoryPoints1 = GetRandomNumber(0,vp1MaxValue);
			history.VictoryPoints2 = GetRandomNumber(0,vp2MaxValue);
			history.Core = "add_core_of = " + TAG;
			if (TAG != secondTAG) {
				history.Core2 = "add_core_of = " + secondTAG;
			}
			history.Buildings = PopulateBuildings();

			return new State {
				StateId = GetStateId(counter, stateId),
				StateName = GetStateName(importedObjects.ImportedNames),
				Manpower = manpower,
				StateCategory = GetStateCategory(manpower),
				BuildingMaxLevelFactor = (double)(GetRandomNumber(0,100)/100), //get as number between 0 and 1
				Resources = PopulateResources(),
				History = history
			};
		}

		public static int GetStateId(int counter, int stateId)
		{
			if (stateId != 0) 
			{
				return stateId;
			} 
			else {
				return counter+1 ;
			}
		}

		public static string GetStateName(List<string> importedNames)
		{
			string retval;

			if (importedNames.Count > 0) 
			{
				var number = GetRandomNumber (0, importedNames.Count);
				retval = importedNames[number];
				importedNames.Remove (retval);
				return retval;
			}

			return GenerateStateName();
		}

		public static string GenerateStateName()
		{
			string [] input = {"Watkins","Atreides","Harkonnen","Harlan","Davis","Said","Lenin","Shield","Needle","Fanon","Gilroy","Mieville","Watts","Horne","Eilenberger","van Reybrouch","Aitmatov","Nkrumah","Ayers","Al-Mansoor","Liang"};
			var nouns = new List<string> (input);
			var enumNumber = GetRandomNumber (0, geographyMaxValue);
			var nounNumber = GetRandomNumber (0, nouns.Count);
			return nouns[nounNumber]+" "+(Geography)enumNumber;
		}

		public static StateCategories GetStateCategory(int manpower)
		{
			if (manpower < 50000) 
			{
				var number = GetRandomNumber (0, 1);
				return (StateCategories)number;
			}

			if (manpower >= 50000 && manpower < 250000) 
			{
				var number = GetRandomNumber (4, 5);
				return (StateCategories)number;
			}


			if (manpower >= 250000 && manpower < 750000) 
			{
				var number = GetRandomNumber (6, 7);
				return (StateCategories)number;
			}

			if (manpower >= 750000 && manpower < 1000000) 
			{
				var number = GetRandomNumber (8, 9);
				return (StateCategories)number;
			}

			if (manpower >= 1000000) 
			{
				var number = GetRandomNumber (10, 11);
				return (StateCategories)number;
			}

			return StateCategories.pastoral; //this shouldn't happen, but just incase.
		}

		public static Dictionary<string,int> PopulateResources()
		{
			Dictionary<string,int> retVal = new Dictionary<string,int>(); 
			string [] input = {"oil", "aluminium", "rubber", "tungsten", "steel", "chromium"};
			var resources = new List<string> (input);
			foreach (var resource in resources) 
			{
				retVal.Add(resource, GetRandomNumber(0,50));
			}

			return retVal;
		}

		public static Dictionary<string,int> PopulateBuildings()
		{
			Dictionary<string,int> retVal = new Dictionary<string,int>();
			string [] input = {"infrastructure", "air_base", "naval_base", "bunker", "coastal_bunker"};
			var buildings = new List<string> (input);
			foreach (var building in buildings) 
			{
				retVal.Add(building, GetRandomNumber(buildingLowerBound,buildingMax10));
			}

			retVal.Add("arms_factory", GetRandomNumber(buildingLowerBound,buildingMax20));
			retVal.Add("industrial_complex", GetRandomNumber(buildingLowerBound,buildingMax20));
			retVal.Add("dockyard", GetRandomNumber(buildingLowerBound,buildingMax20));
			retVal.Add("anti_air_building", GetRandomNumber(buildingLowerBound,buildingMax5));
			retVal.Add("radar_station", GetRandomNumber(buildingLowerBound,buildingMax3));

			return retVal;
		}

		public static string GenerateFileName (int stateId, string stateName)
		{
			return stateId + " " + stateName + ".txt";
		}
	}
}