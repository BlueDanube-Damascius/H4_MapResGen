using System;

using H4_MapResGen.Entities.Definitions;

namespace H4_MapResGen
{

	public class ProvinceDefinition
	{
		public int ProvId { get; set; }

		public string RGBvalue { get; set; }

		public LandSeaLake Humidity { get; set; }

		public bool IsCoastal { get; set; }

		public Terrain TerrainType { get; set; }

		public int ContinentId { get; set; }
	}
}