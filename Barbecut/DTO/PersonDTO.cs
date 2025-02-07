using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Barbecut.DTO
{
	public class PersonDTO
	{
		public int IdPerson { get; set; }

		public int Row { get; set; }

		public string Name { get; set; } = string.Empty;

		public List<ItemDTO> ConsumeListItems { get; set; } = new List<ItemDTO>();

		public int ConsumeItemCount => ConsumeListItems.Count;

	}
}
