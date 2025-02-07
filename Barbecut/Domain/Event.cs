using SQLite;
using SQLiteNetExtensions.Attributes;

namespace Barbecut.Domain
{
	public class Event
	{
		[PrimaryKey, AutoIncrement]
		public int IdEvent { get; set; }
		public string Name { get; set; } = string.Empty;
		public string Description { get; set; } = string.Empty;
		public DateTime Date { get; set; } = DateTime.Now;
		public TimeSpan Time { get; set; }
		public string Location { get; set; } = string.Empty;

		[Ignore]
		public List<Item> ListItems { get; set; } = new List<Item>();

		[TextBlob("ListPerson")]
		public List<Person> ListPerson { get; set; } = new List<Person>();
	}
}
