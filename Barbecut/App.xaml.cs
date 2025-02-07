using Barbecut.Domain;
using Barbecut.Pages.Events;
using Barbecut.Pages.Home;
using Barbecut.Repositories;

namespace Barbecut
{
	public partial class App : Application
	{
		public static Repository<Event> EventRepository { get; private set; } = new Repository<Event>();
		public static Repository<Person> PersonRepository { get; private set; } = new Repository<Person>();
		public static Repository<Item> ItemRepository { get; private set; } = new Repository<Item>();
		public static Repository<ItemPersonBought> ItemPersonBoughtRepository { get; private set; } = new Repository<ItemPersonBought>();

		public App()
		{
			InitializeComponent();
			var navigationPage = new NavigationPage();
			MainPage = new NavigationPage(new HomePage());
		}
	}
}
