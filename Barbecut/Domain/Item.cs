using SQLite;

namespace Barbecut.Domain
{
	public class Item
	{
		[PrimaryKey, AutoIncrement]
		public int IdItem { get; set; }

		public string Description { get; set; } = string.Empty;

		public decimal Price { get; set; }

		public int Units { get; set; }

		[Indexed]
		public int EventId { get; set; }
	}
}
