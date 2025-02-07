using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Barbecut.DTO
{
    public class ItemPersonBoughtDTO
    {
		public int IdEvent { get; set; }
		public string Item { get; set; } = string.Empty;
		public string Person { get; set; } = string.Empty;
	}
}
