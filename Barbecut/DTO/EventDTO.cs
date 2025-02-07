using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Barbecut.DTO
{
    public class EventDTO
    {
		public int IdEvent { get; set; }
		public string Name { get; set; } = string.Empty;
		public string Description { get; set; } = string.Empty;
		public DateTime Date { get; set; } = DateTime.Now;
		public TimeSpan Time { get; set; }
		public string Location { get; set; } = string.Empty;

		public string Status => DateTime.Now.Date > Date.Date ? "Finished event" : "Active event";
	}
}
