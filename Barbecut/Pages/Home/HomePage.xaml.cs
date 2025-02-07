using Barbecut.DTO;
using Barbecut.Pages.Events;
using Barbecut.Pages.Loading;
using Microsoft.Extensions.Logging;
using System.Collections.ObjectModel;
using CommunityToolkit.Maui.Views;
using Barbecut.Services;

namespace Barbecut.Pages.Home;

public partial class HomePage : ContentPage
{
	private List<Button> _buttonFilterNames = new List<Button>();
	private ObservableCollection<EventDTO> _observableListEvent { get; set; } = new ObservableCollection<EventDTO>();
	private List<EventDTO> _listAllEvents { get; set; } = new List<EventDTO>();
	private List<EventDTO> _listFilterEvents { get; set; } = new List<EventDTO>();
	public WhatsAppService _whatsAppService { get; set; } = new WhatsAppService();
	public ExcelService _excelService { get; set; } = new ExcelService();
	public CalculateCostsService _calculateCostsService { get; set; } = new CalculateCostsService();

	private int _eventsLoadedCount = 0;
	private const int _eventsPageSize = 6;
	private string _searchEventInputText = string.Empty;

	public HomePage()
	{
		InitializeComponent();
		NavigationPage.SetHasNavigationBar(this, false);
		_buttonFilterNames = new List<Button>() { AllButtonFilter, ActiveButtonFilter, FinishedButtonFilter };
		EnableFilterButton(AllButtonFilter);
		CollectionViewEvent.ItemsSource = _observableListEvent;
		InitializeEventList();
	}

	public async void InitializeEventList()
	{
		var events = await App.EventRepository.GetAllAsync();
		_listAllEvents.Clear();
		foreach (var eve in events)
		{
			var eventDTO = new EventDTO()
			{
				IdEvent = eve.IdEvent,
				Date = eve.Date,
				Description = eve.Description,
				Location = eve.Location,
				Name = eve.Name,
				Time = eve.Time
			};

			_listAllEvents.Add(eventDTO);

		}
		FilterObservableList();
	}

	protected override void OnAppearing()
	{
		base.OnAppearing();
		InitializeEventList();
	}

	private async void HelpButton_Clicked(object sender, EventArgs e)
	{
		var helpPage = new HelpPage();
		await Application.Current.MainPage.Navigation.PushAsync(helpPage);
	}

	private async void NewEvent_Clicked(object sender, EventArgs e)
	{
		var loadingPopup = new LoadingPopup();
		this.ShowPopup(loadingPopup);
		var eventPage = new EventPage(0, loadingPopup);
		await Application.Current.MainPage.Navigation.PushAsync(eventPage);
	}


	private void AllButtonFilter_Clicked(object sender, EventArgs e)
	{
		EnableFilterButton(AllButtonFilter);
	}

	private void ActiveButtonFilter_Clicked(object sender, EventArgs e)
	{
		EnableFilterButton(ActiveButtonFilter);
	}

	private void FinishedButtonFilter_Clicked(object sender, EventArgs e)
	{
		EnableFilterButton(FinishedButtonFilter);
	}

	private void EnableFilterButton(Button buttonFilterSelected)
	{
		foreach (var button in _buttonFilterNames)
		{
			if (button == buttonFilterSelected)
			{
				VisualStateManager.GoToState(button, "Selected");
				FilterTitle.Text = $"{button.Text} events";
				FilterObservableList();
			}
			else
			{
				VisualStateManager.GoToState(button, "NotSelected");
			}
		}
	}

	private void FilterObservableList()
	{
		_listFilterEvents = _listAllEvents.Where(e =>
		e.Name.ToLower().Contains(_searchEventInputText.ToLower()) &&
		((FilterTitle.Text == "Active events" && e.Status == "Active event") ||
		(FilterTitle.Text == "Finished events" && e.Status == "Finished event") ||
		(FilterTitle.Text == "All events")))
		.ToList();

		_eventsLoadedCount = 0;
		_observableListEvent.Clear();
		LoadMoreEvents();
	}

	private void LoadMoreEvents()
	{
		var eventsToLoad = _listFilterEvents.Skip(_eventsLoadedCount).Take(_eventsPageSize).ToList();

		foreach (var eventDTO in eventsToLoad)
		{
			_observableListEvent.Add(eventDTO);
		}

		_eventsLoadedCount += eventsToLoad.Count;
		LoadMoreButton.IsVisible = _eventsLoadedCount < _listFilterEvents.Count;
	}

	private void LoadMoreButton_Clicked(object sender, EventArgs e)
	{
		LoadMoreEvents();
	}

	private void FindEvent_Completed(object sender, EventArgs e)
	{
		_searchEventInputText = FindEvent.Text;
		FilterObservableList();
	}

	private async void EventCard_removeEvent(object sender, EventArgs e)
	{
		var eventToRemove = _observableListEvent.FirstOrDefault(eve => eve.IdEvent.ToString() == sender.ToString());
		if (eventToRemove != null)
		{
			bool confirmDelete = await Application.Current.MainPage.DisplayAlert(
			"Confirmation",
			$"Do you really want to delete the event {eventToRemove.Name}?",
			"Yes",
			"No"
			);

			if (confirmDelete)
			{
				await App.EventRepository.DeleteByIdAsync(eventToRemove.IdEvent);
				InitializeEventList();
			}
		}
	}

	private async void EventCard_editEvent(object sender, EventArgs e)
	{
		var eventToEdit = _observableListEvent.FirstOrDefault(eve => eve.IdEvent.ToString() == sender.ToString());
		if (eventToEdit != null)
		{
			var loadingPopup = new LoadingPopup();
			this.ShowPopup(loadingPopup);
			var eventPage = new EventPage(eventToEdit.IdEvent, loadingPopup);
			await Application.Current.MainPage.Navigation.PushAsync(eventPage);
		}
	}

	private async void EventCard_shareCostsEvent(object sender, EventArgs e)
	{
		var eventToShareCosts = _observableListEvent.FirstOrDefault(eve => eve.IdEvent.ToString() == sender.ToString());
		if (eventToShareCosts != null)
		{
			string action = await Application.Current.MainPage.DisplayActionSheet(
			"Select one option to share costs", "Cancelar", null, "WhatsApp", "Excel");

			if (action != "Cancelar" && action != null)
			{
				await ExecuteAction(action, eventToShareCosts);
			}
		}
	}

	private async Task ExecuteAction(string action, EventDTO eventToShareCosts) 
	{
		
		await _calculateCostsService.Calculate(eventToShareCosts);

		switch (action) {
			case "WhatsApp":
				_whatsAppService.ShareConsumption(eventToShareCosts, _calculateCostsService._listPayPersonDTO, _calculateCostsService._listOperationPayDTO);
				break;
			case "Excel":
				_excelService.ShareConsumption(eventToShareCosts, _calculateCostsService._listPayPersonDTO, _calculateCostsService._listOperationPayDTO);
				break;
		}
	}

}