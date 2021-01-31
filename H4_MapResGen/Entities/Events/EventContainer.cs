using System;
using System.Collections.Generic;

namespace H4_MapResGen.Entities.Events
{
	public class EventContainer
	{
		public string id { get; set; }
		public string title { get; set; }
		public string desc { get; set; }
		public List<string> options { get; set; }
	}
}

