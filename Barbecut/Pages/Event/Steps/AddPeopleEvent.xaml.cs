using Barbecut.DTO;
using Barbecut.Pages.Event.Components;
using Barbecut.Pages.Event.Interface;
using Barbecut.Utils;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Text.Json;
using System.Xml.Linq;

namespace Barbecut.Pages.Event.Steps;

public partial class AddPeopleEvent : ContentView, IEventStep
{
	public ObservableCollection<PersonDTO> _observableListPerson { get; set; } = new ObservableCollection<PersonDTO>();
	public List<PersonDTO> _listPersonDTO { get; set; } = new List<PersonDTO>();

	public List<ItemCheckBoxDTO> _listItemCheckBoxDTO { get; set; } = new List<ItemCheckBoxDTO>();


	public AddPeopleEvent()
	{
		InitializeComponent();
		CollectionViewItem.ItemsSource = _observableListPerson;

	}

	public void InitializeStepFields()
	{
		int rowIndex = 1;
		foreach (var person in _listPersonDTO)
		{
			_observableListPerson.Add(new PersonDTO()
			{
				IdPerson = person.IdPerson,
				Name = person.Name,
				ConsumeListItems = person.ConsumeListItems,
				Row = rowIndex++,
			});
		}
		
	}

	private bool ValidateNewPerson(PersonDTO newPerson)
	{
		if (string.IsNullOrWhiteSpace(newPerson.Name))
		{
			Alert.ShowAlert("Error", "The name field is empty.");
			return false;
		}

		if (!newPerson.ConsumeListItems.Any())
		{
			Alert.ShowAlert("Error", "Select at least one item to consume.");
			return false;
		}

		return true;
	}

	private void AddPeopleButton_Clicked(object sender, EventArgs e)
	{
		PersonDTO newPersonDTO = new PersonDTO()
		{
			Name = NewName.Text
		};

		newPersonDTO.ConsumeListItems = _listItemCheckBoxDTO.Where(i => i.IsChecked).Select(i => new ItemDTO()
		{
			IdItem = i.IdItem,
			Description = i.Description,
			Price = i.Price,
			Row = i.Row,
			Units = i.Units
		}).ToList();

		if (!ValidateNewPerson(newPersonDTO))
		{
			return;
		}

		_observableListPerson.Add(newPersonDTO);
		_listPersonDTO.Add(newPersonDTO);

		CleanFields();
	}

	private async void ConsumeButton_Clicked(object sender, EventArgs e)
	{
		var consumeListPage = new ConsumeListPage(_listItemCheckBoxDTO);
		consumeListPage.OkConsumeListButtonEvent += ConsumeListPage_OkConsumeListButtonEvent;
		await Application.Current.MainPage.Navigation.PushAsync(consumeListPage);
	}

	private void ConsumeListPage_OkConsumeListButtonEvent(object sender, EventArgs e)
	{
		var itemsSelected = _listItemCheckBoxDTO.Count(a => a.IsChecked);
		ConsumeButton.Text = $"{itemsSelected} items selected";
	}

	public void CleanFields()
	{
		NewName.Text = string.Empty;
        foreach (var item in _listItemCheckBoxDTO)
        {
			item.IsChecked = false;
        }
		var itemsSelected = _listItemCheckBoxDTO.Count(a => a.IsChecked);
		ConsumeButton.Text = $"{itemsSelected} items selected";
	}

	private void PersonCard_removePersonEvent(object sender, EventArgs e)
	{
		var personToRemove = _observableListPerson.FirstOrDefault(item => item.Row.ToString() == sender.ToString());
		if (personToRemove != null)
		{
			_observableListPerson.Remove(personToRemove);
			ReorderRowObservableList();
			var personDTO = _listPersonDTO.FirstOrDefault(person => person.Name == personToRemove.Name);
			if (personDTO != null)
			{
				_listPersonDTO.Remove(personDTO);
			}
		}
	}

	private void ReorderRowObservableList()
	{
		for (int i = 0; i < _observableListPerson.Count; i++)
		{
			_observableListPerson[i].Row = i + 1;
		}
	}


	private void PersonCard_editPersonEvent(object sender, EventArgs e)
	{
		var personToEdit = _observableListPerson.FirstOrDefault(item => item.Row.ToString() == sender.ToString());
		if (personToEdit != null)
		{
			NewName.Text = personToEdit.Name;

			foreach (var item in _listItemCheckBoxDTO)
			{
				item.IsChecked = personToEdit.ConsumeListItems.Any(c => c.Description.ToLower() == item.Description.ToLower());
			}

			var itemsSelected = _listItemCheckBoxDTO.Count(a => a.IsChecked);
			ConsumeButton.Text = $"{itemsSelected} items selected";
			_observableListPerson.Remove(personToEdit);
			ReorderRowObservableList();
			var personDTO = _listPersonDTO.FirstOrDefault(person => person.Name.ToLower() == personToEdit.Name.ToLower());
			if (personDTO != null)
			{
				_listPersonDTO.Remove(personDTO);
			}
		}
	}
}