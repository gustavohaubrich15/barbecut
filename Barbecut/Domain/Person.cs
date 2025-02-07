using SQLite;
using SQLiteNetExtensions.Attributes;

namespace Barbecut.Domain
{
    public class Person
    {
		[PrimaryKey, AutoIncrement]
		public int IdPerson { get; set; }

		public string Name { get; set; } = string.Empty;

		[TextBlob("ConsumeListItemBlob")]
		public List<Item> ConsumeListItem { get; set; } = new List<Item>();

		public string ConsumeListItemBlob { get; set; } = string.Empty;

		[Indexed]
		public int EventId { get; set; }
	}
}
