using Barbecut.DTO;
using System.Collections.ObjectModel;
using System.Text.Json;

namespace Barbecut.Pages.Event.Components;

public partial class ConsumeListPage : ContentPage
{
	public event EventHandler? OkConsumeListButtonEvent;

	public ObservableCollection<ItemCheckBoxDTO> _observableListCheckBoxItem { get; set; } = new ObservableCollection<ItemCheckBoxDTO>();

	public List<ItemCheckBoxDTO> _listItemCheckBoxDTO { get; set; }

	public ConsumeListPage(List<ItemCheckBoxDTO> listItemCheckBoxDTO)
	{
		InitializeComponent();
		NavigationPage.SetHasNavigationBar(this, false);
		_listItemCheckBoxDTO = listItemCheckBoxDTO;
		foreach(var itemCheckBoxDTO in listItemCheckBoxDTO)
		{
			_observableListCheckBoxItem.Add(itemCheckBoxDTO);
		}
		CollectionViewCheckBoxItem.ItemsSource = _observableListCheckBoxItem;
	}

	private async void OkButton_Clicked(object sender, EventArgs e)
	{
		OkConsumeListButtonEvent?.Invoke(null, e);
		await Application.Current.MainPage.Navigation.PopAsync();
	}

	private void CheckBox_CheckedChanged(object sender, CheckedChangedEventArgs e)
	{
		var checkBox = (CheckBox)sender;
		var item = (ItemCheckBoxDTO)checkBox.BindingContext;
		bool isChecked = e.Value;

		if (item != null)
		{
			var itemCheckBoxDTO = _listItemCheckBoxDTO.FirstOrDefault(i => i.Description == item.Description);
			if (itemCheckBoxDTO != null)
			{
				itemCheckBoxDTO.IsChecked = isChecked;
			}

			var observableItem = _observableListCheckBoxItem.FirstOrDefault(i => i.Description == item.Description);
			if (observableItem != null)
			{
				observableItem.IsChecked = isChecked;
			}
		}
	}
}