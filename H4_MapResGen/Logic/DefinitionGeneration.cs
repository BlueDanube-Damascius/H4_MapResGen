using System;
using System.Collections.Generic;
using System.Linq;

using H4_MapResGen.Entities.Definitions;

namespace H4_MapResGen
{
	public class DefinitionGeneration
	{
		private static readonly string Seperator = (";");

		public static List<ProvinceDefinition> GenerateDefinitions (DefinitionContainer container)
		{
			var retVal = new List<ProvinceDefinition> ();


			LandSeaLake humidity = new LandSeaLake ();
			if (container.LSLinput != 99) { 
				humidity = (LandSeaLake)container.LSLinput; 
			}

			List<string> RGBValues = new List<string>();

			var i = 0;
			while (i < container.provNumber) {
				var RGBValue = GenerateRandomRGBValues ();
				if (!RGBValues.Any().Equals(RGBValue))
					{
						RGBValues.Add (RGBValue);
						i++;
					}
			}

			Dictionary<int,int> ContinentIdValues = new Dictionary<int,int>();
			ContinentIdValues = PopulateContinentIds (container.provId, container.provNumber, container.continents, container.continentsLowerBound);

			int provId = container.provId;
			
			foreach (var value in RGBValues) {
				if (container.LSLinput == 99) {
					humidity = (LandSeaLake)MainClass.GetRandomNumber (0, 3);
				}
				var province = new ProvinceDefinition ();
				province.ProvId = provId;
				province.Humidity = humidity;
				province.IsCoastal = EvaluateIfCoastal (humidity);
				province.TerrainType = PopulateTerrainType (humidity);
				province.RGBvalue = value; 
				province.ContinentId = ContinentIdValues[provId];
				if (humidity == LandSeaLake.Sea){
					province.ContinentId = 0;
				}
				retVal.Add (province);
				provId++;
			}
			return retVal;
		}

		public static string GenerateRandomRGBValues ()
		{
			int redValue = MainClass.GetRandomNumber (0, 255);
			int greenValue = MainClass.GetRandomNumber (0, 255);
			int blueValue = MainClass.GetRandomNumber (0, 255);
			return redValue + Seperator + greenValue + Seperator + blueValue;
		}

		public static bool EvaluateIfCoastal (LandSeaLake Humidity)
		{
			var number = MainClass.GetRandomNumber (0, 2);
			var retVal = false; //not coastal
			if ( number == 1 && Humidity != LandSeaLake.Lake) { //a coastal lake is sea!
				retVal = true; //coastal
			}
			return retVal;
		}

		public static Terrain PopulateTerrainType (LandSeaLake Humidity)
		{
			switch (Humidity) {
			case LandSeaLake.Sea:
				return Terrain.ocean;
			case LandSeaLake.Lake:
				return Terrain.lakes;
			default:
				return (Terrain)MainClass.GetRandomNumber (2, 9);
			}
		}

		public static Dictionary<int,int> PopulateContinentIds(int provId, int provNumber, int continents, int continentsLowerBound)
		{
			var i = 0;
			var j = continents;
			var k = continentsLowerBound;
			var continentNumber = (continents - continentsLowerBound);
				if(continentNumber == 0 ){ continentNumber = 1;}
					
			var provincesAdjusted = provId + provNumber;
			var provincesPerContinent = (provNumber / continentNumber);
			var continentIDlist = new List<int> ();
			while (j >= k) {
				continentIDlist.Add (j);
				j--;
			}

			var retVal = new Dictionary<int,int> ();
			foreach (var continentID in continentIDlist) 
				{	while (i < provincesPerContinent) {
					retVal.Add (provId, continentID);
					provId++;
					i++;
				}
				i = 0;
			}

			//just incase pPC is less than total due to rounding, add to last continent in list
			while (provId < provincesAdjusted) {
				retVal.Add(provId, continents);
					provId++;
			}

			return retVal;
		}

		public static List<string> SerialisedObjects (List<ProvinceDefinition> definitions)
		{
			var retVal = new List<string> ();
			foreach (var province in definitions) {
				var definition = province.ProvId + Seperator + province.RGBvalue + Seperator + province.Humidity + Seperator + province.IsCoastal + Seperator + province.TerrainType + Seperator + province.ContinentId + Seperator;
				retVal.Add (definition);
			}
					return retVal;
		}
	}
}