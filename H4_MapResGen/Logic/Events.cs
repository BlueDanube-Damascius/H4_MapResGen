using System;
using System.Collections.Generic;

using H4_MapResGen.Entities.Events;

namespace H4_MapResGen.Logic
{
	public class Events
	{
		public static void EventsMain()
		{
			var events = new List<EventContainer>();
			Console.WriteLine ("Define Namespace");
			var _namespace = Console.ReadLine();

			Console.WriteLine ("Input number of events to generate");
			var input = Console.ReadLine ();
			int eventno = 1;
			Int32.TryParse (input, out eventno);
			int i = 1;
			while (i <= eventno) {
				Console.WriteLine (string.Format("How many options are expected for event {0}?",i));
				int optionno = Convert.ToInt32(Console.ReadLine ());
				var _event = new EventContainer ();
				_event.id = string.Format ("{0}.{1}", _namespace, i);
				_event.title = string.Format ("{0}.{1}.t", _namespace, i);
				_event.desc = string.Format ("{0}.{1}.d", _namespace, i);
				_event.options = new List<string> ();
				var j = 0;
				while (j < optionno) {
					var option = String.Format("{0}.{1}.{2}", _namespace, i, Resources.OptionNames[j]);  
					_event.options.Add (option);
					j++;
				}
				events.Add (_event);
				i++;
			}
			Console.WriteLine ("Generating event frameworks...");
			var serialisedobjs = EventGeneration.EventSerialise (events, _namespace);
			Console.WriteLine ("Saving output to local folder.");
			FileManagement.SaveEventsToFile (serialisedobjs, _namespace);

			Console.WriteLine ("Press Any Key to Kill.");
			Console.ReadKey ();
		
		}
	}
}

