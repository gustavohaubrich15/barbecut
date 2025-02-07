using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Barbecut.DTO
{
	public class PayPersonDTO
	{
		public string Item { get; set; } = string.Empty;
		public string NamePerson { get; set; } = string.Empty;
		public decimal ItemTotal { get; set; }
		public List<string> ListConsumePersonItem { get; set; } = new List<string>();
		public decimal PriceForPerson => ListConsumePersonItem.Count > 0 ? ItemTotal / ListConsumePersonItem.Count : 0;
	}

}
