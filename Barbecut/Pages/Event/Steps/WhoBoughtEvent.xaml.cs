using Barbecut.DTO;
using System.Collections.ObjectModel;

namespace Barbecut.Pages.Event.Steps;

public partial class WhoBoughtEvent : ContentView
{
	public ObservableCollection<ItemPersonBoughtDTO> _observableListItemPersonBought { get; set; } = new ObservableCollection<ItemPersonBoughtDTO>();

	public ObservableCollection<PersonDTO> _observableListPerson { get; set; } = new ObservableCollection<PersonDTO>();

	public List<PersonDTO> _listPersonDTO { get; set; } = new List<PersonDTO>();

	public List<ItemPersonBoughtDTO> _listItemPersonBoughtDTO { get; set; } = new List<ItemPersonBoughtDTO>();

	public WhoBoughtEvent()
	{
		InitializeComponent();
	}

	public void InitializeStepFields()
	{
		_observableListItemPersonBought.Clear();
		foreach (var item in _listItemPersonBoughtDTO)
		{
			_observableListItemPersonBought.Add(new ItemPersonBoughtDTO()
			{
				IdEvent = item.IdEvent,
				Item = item.Item,
				Person = item.Person
			});
		}
	}

	public void InitializePicker()
	{
		_observableListPerson.Clear();
		foreach (var person in _listPersonDTO)
		{
			_observableListPerson.Add(new PersonDTO()
			{
				Name = person.Name
			});
		}
		CreateCards();

	}

	private void CreateCards()
	{
		CardsContainer.Children.Clear();

		foreach (var item in _observableListItemPersonBought)
		{
			var picker = new Picker
			{
				ItemsSource = _observableListPerson,
				ItemDisplayBinding = new Binding("Name"),
				SelectedItem = _observableListPerson.Where(a => a.Name.ToLower() == item.Person.ToLower()).FirstOrDefault(),
				VerticalOptions = LayoutOptions.Center,

			};

			picker.SelectedIndexChanged += (sender, e) =>
			{
				var selectedPicker = sender as Picker;
				if (selectedPicker != null && selectedPicker.SelectedItem != null)
				{

					var selectedPerson = selectedPicker.SelectedItem as PersonDTO;
					var existingItem = _listItemPersonBoughtDTO.FirstOrDefault(i => i.Item.ToLower() == item.Item.ToLower());

					if (existingItem != null)
					{
						existingItem.Person = selectedPerson?.Name ?? string.Empty;

						Console.WriteLine($"Pessoa selecionada (atualizada): {existingItem.Person}");
					}
					else
					{

						_listItemPersonBoughtDTO.Add(new ItemPersonBoughtDTO
						{
							IdEvent = item.IdEvent,
							Item = item.Item,
							Person = selectedPerson?.Name ?? string.Empty,
						});

						Console.WriteLine($"Pessoa selecionada (nova): {selectedPerson?.Name}");
					}
				}
			};

			var card = new Border
			{
				Padding = new Thickness(10),
				Style = (Style)Application.Current.Resources["BorderCard"],
				WidthRequest = 300,
				Content = new FlexLayout
				{
					Direction = Microsoft.Maui.Layouts.FlexDirection.Row,
					JustifyContent = Microsoft.Maui.Layouts.FlexJustify.SpaceAround,
					AlignItems = Microsoft.Maui.Layouts.FlexAlignItems.Center,
					Children =
					{
                        new Label { Text = item.Item, VerticalOptions = LayoutOptions.Center },
						new Image { Source = "handright.svg", HeightRequest=20, WidthRequest=20, VerticalOptions = LayoutOptions.Center },
						picker
					}
				}
			};

			CardsContainer.Children.Add(card);
		}
	}

}