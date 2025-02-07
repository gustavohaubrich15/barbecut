using Barbecut.DTO;
using System.Collections.ObjectModel;
using System.Xml.Linq;

namespace Barbecut.Pages.Event.Steps;

public partial class DoneEvent : ContentView
{
	public EventDTO _eventDTO { get; set; } = new EventDTO();
	public List<PersonDTO> _listPersonDTO { get; set; } = new List<PersonDTO>();
	public List<ItemDTO> _listItemEventDTO { get; set; } = new List<ItemDTO>();
	public List<ItemPersonBoughtDTO> _listItemPersonBoughtDTO { get; set; } = new List<ItemPersonBoughtDTO>();
	public ObservableCollection<ObservableCollectionView> _confirmedPeople { get; set; } = new ObservableCollection<ObservableCollectionView>();
	public ObservableCollection<ObservableCollectionView> _foodAndDrinks { get; set; } = new ObservableCollection<ObservableCollectionView>();
	public ObservableCollection<ObservableCollectionView> _whoBought { get; set; } = new ObservableCollection<ObservableCollectionView>();

	public DoneEvent()
	{
		InitializeComponent();
		CollectionViewConfirmedPeople.ItemsSource = _confirmedPeople;
		CollectionViewFoodAndDrinks.ItemsSource = _foodAndDrinks;
		CollectionViewWhoBought.ItemsSource = _whoBought;
	}

	public void InitializeStepFields()
	{
		_confirmedPeople.Clear();
		_foodAndDrinks.Clear();
		_whoBought.Clear();
		Description.Text = $"• {_eventDTO.Description}";
		Location.Text = $"• {_eventDTO.Location}";
		Date.Text = $"• {_eventDTO.Date.ToString("dd/MM/yyyy")}";
		Time.Text = $"• {_eventDTO.Time.Hours:D2}:{_eventDTO.Time.Minutes:D2}";

		foreach (var person in _listPersonDTO) {
			_confirmedPeople.Add(new ObservableCollectionView()
			{
				LabelText = $"• {person.Name}"
			});
		}

		foreach (var item in _listItemEventDTO)
		{
			_foodAndDrinks.Add(new ObservableCollectionView()
			{
				LabelText = $"• {item.Description.ToUpper()}; Price - R$ {item.Price}; Units - {item.Units}"
			});
		}

		foreach (var itemPersonBought in _listItemPersonBoughtDTO)
		{
			_whoBought.Add(new ObservableCollectionView()
			{
				LabelText = $"• {itemPersonBought.Person} - {itemPersonBought.Item}"
			});
		}

		if(_listItemPersonBoughtDTO.Count == 0)
		{
			_whoBought.Add(new ObservableCollectionView()
			{
				LabelText = $"• No items were marked as purchased by a person"
			});
		}

		TotalEvent.Text = $"R$ {_listItemEventDTO.Sum(a => a.Price * a.Units):F2}";
		TotalPeople.Text = _listPersonDTO.Count.ToString();

	}

	public class ObservableCollectionView
	{
		public string LabelText { get; set; } = string.Empty ;
	}

}