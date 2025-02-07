using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Barbecut.Domain
{
	public class ItemPersonBought
	{
		[PrimaryKey, AutoIncrement]
		public int IdItemPersonBought { get; set; }

		public string Item { get; set; } = string.Empty;

		public string Person { get; set; } = string.Empty;

		[Indexed]
		public int EventId { get; set; }
	}
}
