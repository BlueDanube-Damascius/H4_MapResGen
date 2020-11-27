using System;
using System.Collections.Generic;

namespace H4_MapResGen.Entities
{
	public class State
	{
		public int StateId { get; set;}
		public string StateName { get; set; }
		public int Manpower { get; set; }
		public StateCategories StateCategory { get; set; }
		public double BuildingMaxLevelFactor { get; set; }
		public Dictionary<string,int> Resources { get; set;}
		public History History { get; set; }
	}
}