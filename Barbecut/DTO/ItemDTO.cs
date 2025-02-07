using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Barbecut.DTO
{
	public class ItemDTO
	{
		public int IdItem { get; set; }

		public int Row { get; set; }

		public string Description { get; set; } = string.Empty;

		public decimal Price { get; set; }

		public int Units { get; set; }
	}
}
