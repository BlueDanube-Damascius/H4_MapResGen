using System;
using System.Collections.Generic;

namespace H4_MapResGen.Entities
{
	public class History
	{
		public string Owner { get; set;}
		public int VictoryPoints1 { get; set;}
		public int VictoryPoints2 { get; set;}
		public string Core { get; set;}
		public string Core2 { get; set; }
		public Dictionary<string,int> Buildings { get; set;}
	}
}

