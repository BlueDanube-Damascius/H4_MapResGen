using System;
using System.Collections.Generic;

namespace H4_MapResGen.Entities.States
{
	public class StatesContainer
	{
		public List<string> ImportedNames { get; set; }
		public List<string> ImportedTAGs { get; set; }
		public List<string> ImportedProvinces { get; set; }
		public List<string> ImportedDefaults { get; set; }
		public bool TwoCores { get; set; }
	}
}