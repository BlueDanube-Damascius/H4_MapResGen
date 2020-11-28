using System;

namespace H4_MapResGen.Entities
{
	public enum Geography
	{
		Area = 0,
		Plateau = 1,
		Plains = 2,
		Highlands = 3,
		Range = 4,
		Wall = 5,
		Hive = 6,
		Wastes = 7,
		Massif = 8,
		Basin = 9, 
		Upland = 10,
		Lowlands = 11,
		Belt = 12,
		Foothills = 13,
		Oldland = 14,
		Shield = 15,
		Littoral = 16,
		Hills = 17,
		Rise = 18,
		Desert = 19,
		Isthmus = 20,
		Peninsula = 21
			// if you add values to this you must update geographyMaxValue in Generation.cs
	}
}