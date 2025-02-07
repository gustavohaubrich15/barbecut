using Barbecut.Domain;
using Barbecut.DTO;
using Barbecut.Pages.Event.Interface;
using Barbecut.Pages.Events;
using Barbecut.Utils;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Windows.Input;
using static Barbecut.Pages.Event.Components.ItemCard;

namespace Barbecut.Pages.Event.Steps;

public partial class FoodAndDrinksEvent : ContentView, IEventStep
{
	public ObservableCollection<ItemDTO> _observableListItems { get; set; } = new ObservableCollection<ItemDTO>();
	public List<ItemDTO> _listItemEventDTO { get; set; } = new List<ItemDTO>();

	public FoodAndDrinksEvent()
	{
		InitializeComponent();
		CollectionViewItem.ItemsSource = _observableListItems;
	}

	public void InitializeStepFields()
	{
		int rowIndex = 1;
		foreach (var item in _listItemEventDTO)
        {
			_observableListItems.Add(new ItemDTO()
			{
				IdItem = item.IdItem,
				Description = item.Description,
				Price = item.Price,
				Units = item.Units,
				Row = rowIndex++,
			});
        }
    }

	private void NewItemPrice_Completed(object sender, EventArgs e)
	{
		if (string.IsNullOrEmpty(NewItemPrice.Text)) {
			return;
		}

		string onlyNumbers = new string(NewItemPrice.Text.Where(char.IsDigit).ToArray());

		if (onlyNumbers.Length > 0)
		{
			decimal value = decimal.Parse(onlyNumbers) / 100;

			NewItemPrice.Text = string.Format(CultureInfo.CurrentCulture, "R$ {0:N2}", value);
		}

	}

	private bool ValidateNewItem(ItemDTO itemDTO)
	{
		if (string.IsNullOrWhiteSpace(itemDTO.Description))
		{
			Alert.ShowAlert("Error", "The description field is empty.");
			return false;
		}

		if (itemDTO.Price == 0M)
		{
			Alert.ShowAlert("Error", "The price field is empty.");
			return false;
		}

		if (itemDTO.Units == 0)
		{
			Alert.ShowAlert("Error", "The units  field is empty.");
			return false;
		}

		return true;
	}

	private void AddItemButton_Clicked(object sender, EventArgs e)
	{
		decimal newPrice = 0;
		var newItemPrice = NewItemPrice.Text != null ? NewItemPrice.Text.Replace("R$", "") : NewItemPrice.Text;
		decimal.TryParse(newItemPrice,  out newPrice);
		int newUnits = 0;
		int.TryParse(NewItemUnits.Text, out newUnits);

		ItemDTO newItemDTO = new ItemDTO() {
			Row = _observableListItems.Count + 1,
			Description = NewItemDescription.Text,
			Price = newPrice,
			Units = newUnits,
		};

		if (!ValidateNewItem(newItemDTO))
		{
			return;
		}

		_observableListItems.Add(newItemDTO);
		_listItemEventDTO.Add(new ItemDTO()
		{
			IdItem = 0,
			Description = newItemDTO.Description,
			Price = newItemDTO.Price,
			Units = newItemDTO.Units
		});
		
		CleanFields();
	}

	private void ItemCard_RemoveItemEvent(object sender, EventArgs e)
	{
		var itemToRemove = _observableListItems.FirstOrDefault(item => item.Row.ToString() == sender.ToString());
		if (itemToRemove != null)
		{
			_observableListItems.Remove(itemToRemove);
			ReorderRowObservableList();
			var itemDTO = _listItemEventDTO.FirstOrDefault(item => item.Description == itemToRemove.Description);
			if (itemDTO != null) { 
				_listItemEventDTO.Remove(itemDTO);
			}
		}
	}

	private void ReorderRowObservableList()
	{
		for (int i = 0; i < _observableListItems.Count; i++)
		{
			_observableListItems[i].Row = i + 1;
		}
	}

	private void CleanFields()
	{
		NewItemDescription.Text = string.Empty;
		NewItemPrice.Text = string.Empty;
		NewItemUnits.Text = string.Empty;
	}


}