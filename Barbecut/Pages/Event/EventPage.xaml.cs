using Barbecut.Domain;
using Barbecut.DTO;
using Barbecut.Pages.Home;
using Barbecut.Services;
using Newtonsoft.Json;
using System.Collections.Generic;
using CommunityToolkit.Maui.Views;
using Barbecut.Pages.Loading;

namespace Barbecut.Pages.Events;
public partial class EventPage : ContentPage
{
	private string ActiveStep = "BasicInformation";
	public EventDTO _eventDTO { get; set; } = new EventDTO();
	public List<ItemDTO> _listItemEventDTO { get; set; } = new List<ItemDTO>();
	public List<PersonDTO> _listPersonDTO { get; set; } = new List<PersonDTO>();
	public List<ItemCheckBoxDTO> _listItemCheckBoxDTO { get; set; } = new List<ItemCheckBoxDTO>();
	public List<ItemPersonBoughtDTO> _listItemPersonBoughtDTO { get; set; } = new List<ItemPersonBoughtDTO>();

	public WhatsAppService _whatsAppService { get; set; } = new WhatsAppService();

	public EventPage(int idEvent, LoadingPopup? loadingPopup)
	{
		InitializeComponent();
		NavigationPage.SetHasNavigationBar(this, false);
		Dispatcher.Dispatch(LoadActiveStep);
		Dispatcher.Dispatch(LoadActiveButtons);

		LoadInitialValues(idEvent, loadingPopup);
	}

	private async void LoadInitialValues(int idEvent, LoadingPopup? loadingPopup)
	{
		if(idEvent > 0)
		{
			_eventDTO.IdEvent = idEvent;
			var eventDomain = await App.EventRepository.GetByIdAsync(idEvent);
			_eventDTO.Name = eventDomain.Name;
			_eventDTO.Description = eventDomain.Description;
			_eventDTO.Date = eventDomain.Date;
			_eventDTO.Time = eventDomain.Time;
			_eventDTO.Location = eventDomain.Location;

			var listItem = await App.ItemRepository.GetItemsByEventIdAsync(idEvent);
			_listItemEventDTO.AddRange(listItem.Select(a => new ItemDTO()
			{
				Description = a.Description,
				IdItem = a.IdItem,
				Price = a.Price,
				Units = a.Units
			}).ToList());

			var listPersonDomain = await App.PersonRepository.GetPersonsByEventIdAsync(_eventDTO.IdEvent);

			_listPersonDTO.AddRange(listPersonDomain.Select(a => new PersonDTO()
			{
				IdPerson = a.IdPerson,
				Name = a.Name,
				ConsumeListItems = JsonConvert.DeserializeObject<List<Item>>(a.ConsumeListItemBlob).Select(b => new ItemDTO()
				{
					IdItem = b.IdItem,
					Price = b.Price,
					Units = b.Units,
					Description = b.Description

				}).ToList()
			}).ToList());

			var listItemPersonBoughtDomain = await App.ItemPersonBoughtRepository.GetItemPersonBoughtByEventIdAsync(_eventDTO.IdEvent);
			_listItemPersonBoughtDTO.AddRange(listItemPersonBoughtDomain.Select(a => new ItemPersonBoughtDTO()
			{
				IdEvent = a.EventId,
				Item = a.Item,
				Person = a.Person
			}).ToList());
			foreach (var item in _listItemEventDTO)
			{
				bool exists = _listItemPersonBoughtDTO.Any(b => b.Item == item.Description);

				if (!exists)
				{
					_listItemPersonBoughtDTO.Add(new ItemPersonBoughtDTO()
					{
						IdEvent = _eventDTO.IdEvent,
						Item = item.Description
					});
				}
			}
		}

		BasicInformationEventView._eventDTO = _eventDTO;
		BasicInformationEventView.InitializeStepFields();
		FoodAndDrinksEventView._listItemEventDTO = _listItemEventDTO;
		FoodAndDrinksEventView.InitializeStepFields();
		AddPeopleEventView._listPersonDTO = _listPersonDTO;
		AddPeopleEventView._listItemCheckBoxDTO = _listItemCheckBoxDTO;
		AddPeopleEventView.InitializeStepFields();
		WhoBoughtView._listItemPersonBoughtDTO = _listItemPersonBoughtDTO;
		WhoBoughtView.InitializeStepFields();
		if(loadingPopup != null)
		{
			await loadingPopup.CloseAsync();
		}
	}

	private void LoadActiveStep()
	{
		var activeStyle = Application.Current?.Resources.TryGetValue("ActiveProgressEventBorderWithIcon", out var activeStyleResource) == true
					? (Style)activeStyleResource
					: null;

		var disableStyle = Application.Current?.Resources.TryGetValue("DisableProgressEventBorderWithIcon", out var disableStyleResource) == true
					? (Style)disableStyleResource
					: null;

		if (activeStyle == null || disableStyle == null)
		{
			return;
		}

		switch (ActiveStep)
		{
			case "BasicInformation":
				LabelStepOneProgress.Text = "1";
				BorderStepOneProgress.Style = activeStyle;
				BorderStepTwoProgress.Style = disableStyle;
				BorderStepThreeProgress.Style = disableStyle;
				BorderStepFourProgress.Style = disableStyle;
				BorderStepFiveProgress.Style = disableStyle;
				TitleEventStep.Text = "Basic Information";
				break;
			case "FoodAndDrinks":
				LabelStepOneProgress.Text = "✔";
				LabelStepTwoProgress.Text = "2";
				BorderStepOneProgress.Style = activeStyle;
				BorderStepTwoProgress.Style = activeStyle;
				BorderStepThreeProgress.Style = disableStyle;
				BorderStepFourProgress.Style = disableStyle;
				BorderStepFiveProgress.Style = disableStyle;
				TitleEventStep.Text = "Food And Drinks";
				break;
			case "AddPeople":
				LabelStepTwoProgress.Text = "✔";
				LabelStepThreeProgress.Text = "3";
				BorderStepOneProgress.Style = activeStyle;
				BorderStepTwoProgress.Style = activeStyle;
				BorderStepThreeProgress.Style = activeStyle;
				BorderStepFourProgress.Style = disableStyle;
				BorderStepFiveProgress.Style = disableStyle;
				TitleEventStep.Text = "Add people";
				break;
			case "WhoBought":
				LabelStepThreeProgress.Text = "✔";
				LabelStepFourProgress.Text = "4";
				BorderStepOneProgress.Style = activeStyle;
				BorderStepTwoProgress.Style = activeStyle;
				BorderStepThreeProgress.Style = activeStyle;
				BorderStepFourProgress.Style = activeStyle;
				BorderStepFiveProgress.Style = disableStyle;
				TitleEventStep.Text = "Who Bought";
				break;
			case "Done":
				LabelStepFourProgress.Text = "✔";
				LabelStepFiveProgress.Text = "5";
				BorderStepOneProgress.Style = activeStyle;
				BorderStepTwoProgress.Style = activeStyle;
				BorderStepThreeProgress.Style = activeStyle;
				BorderStepFourProgress.Style = activeStyle;
				BorderStepFiveProgress.Style = activeStyle;
				TitleEventStep.Text = "Summary";
				break;
		}
	}

	private async void LoadActiveButtons()
	{
		switch (ActiveStep)
		{
			case "BasicInformation":
				StepsButtonsLayout.IsVisible = true;
				SummaryAndEndButtonsLayout.IsVisible = false;
				StepsButtonsLayout.Opacity = 1;
				SummaryAndEndButtonsLayout.Opacity = 0;
				StepsButtonsLayoutReturn.IsVisible = true;
				StepsButtonsLayoutBackStep.IsVisible = false;
				StepsButtonsLayoutBackStep.Opacity = 0;
				await StepsButtonsLayoutReturn.FadeTo(1, 500);
				await StepsButtonsLayoutNextStep.FadeTo(1, 500);
				break;
			case "FoodAndDrinks":
				StepsButtonsLayout.IsVisible = true;
				SummaryAndEndButtonsLayout.IsVisible = false;
				StepsButtonsLayout.Opacity = 1;
				SummaryAndEndButtonsLayout.Opacity = 0;
				StepsButtonsLayoutReturn.IsVisible = false;
				StepsButtonsLayoutBackStep.IsVisible = true;
				StepsButtonsLayoutReturn.Opacity = 0;
				await StepsButtonsLayoutBackStep.FadeTo(1, 500);
				await StepsButtonsLayoutNextStep.FadeTo(1, 500);
				break;
			case "AddPeople":
				StepsButtonsLayout.IsVisible = true;
				SummaryAndEndButtonsLayout.IsVisible = false;
				StepsButtonsLayout.Opacity = 1;
				SummaryAndEndButtonsLayout.Opacity = 0;
				StepsButtonsLayoutReturn.Opacity = 0;
				await StepsButtonsLayoutBackStep.FadeTo(1, 500);
				await StepsButtonsLayoutNextStep.FadeTo(1, 500);
				break;
			case "WhoBought":
				StepsButtonsLayout.IsVisible = true;
				SummaryAndEndButtonsLayout.IsVisible = false;
				StepsButtonsLayout.Opacity = 1;
				SummaryAndEndButtonsLayout.Opacity = 0;
				StepsButtonsLayoutReturn.Opacity = 0;
				await StepsButtonsLayoutBackStep.FadeTo(1, 500);
				await StepsButtonsLayoutNextStep.FadeTo(1, 500);
				break;
			case "Done":
				StepsButtonsLayout.Opacity = 0;
				StepsButtonsLayout.IsVisible = false;
				SummaryAndEndButtonsLayout.IsVisible = true;
				await SummaryAndEndButtonsLayout.FadeTo(1, 500);
				break;
		}
	}

	private async void LoadBackProgress()
	{
		switch (ActiveStep)
		{
			case "FoodAndDrinks":
				await StepOneProgressBar.ProgressTo(0, 2000, Easing.Linear);
				break;
			case "AddPeople":
				await StepTwoProgressBar.ProgressTo(0, 2000, Easing.Linear);
				break;
			case "WhoBought":
				await StepThreeProgressBar.ProgressTo(0, 2000, Easing.Linear);
				break;
			case "Done":
				await StepFourProgressBar.ProgressTo(0, 2000, Easing.Linear);
				break;
		}
	}

	private async void LoadNextProgress()
	{
		switch (ActiveStep)
		{
			case "BasicInformation":
				await StepOneProgressBar.ProgressTo(1, 2000, Easing.Linear);
				break;
			case "FoodAndDrinks":
				await StepTwoProgressBar.ProgressTo(1, 2000, Easing.Linear);
				break;
			case "AddPeople":
				await StepThreeProgressBar.ProgressTo(1, 2000, Easing.Linear);
				break;
			case "WhoBought":
				await StepFourProgressBar.ProgressTo(1, 2000, Easing.Linear);
				break;
		}
	}

	private async void SaveStep()
	{
		var eventDomain = await App.EventRepository.GetByIdAsync(_eventDTO.IdEvent);
		var listPersonDomain = await App.PersonRepository.GetPersonsByEventIdAsync(_eventDTO.IdEvent);

		switch (ActiveStep)
		{
			case "BasicInformation":
				break;

			case "FoodAndDrinks":
				if (eventDomain == null)
				{
					var newEventDomain = new Domain.Event()
					{
						Description = _eventDTO.Description,
						Location = _eventDTO.Location,
						Date = _eventDTO.Date,
						Name = _eventDTO.Name,
						Time = _eventDTO.Time,
					};
					await App.EventRepository.CreateAsync(newEventDomain);
					_eventDTO.IdEvent = newEventDomain.IdEvent;
				}
				else
				{
					eventDomain.Description = _eventDTO.Description;
					eventDomain.Location = _eventDTO.Location;
					eventDomain.Date = _eventDTO.Date;
					eventDomain.Name = _eventDTO.Name;
					eventDomain.Time = _eventDTO.Time;
					await App.EventRepository.UpdateAsync(eventDomain);
				}
				break;

			case "AddPeople":
				if(eventDomain != null)
				{
					await App.ItemRepository.DeleteItemsByEventIdAsync(eventDomain.IdEvent);
					var newListItemEventDTO = new List<ItemDTO>();
					_listItemEventDTO = FoodAndDrinksEventView._listItemEventDTO;

					foreach (var itemDTO in _listItemEventDTO)
					{
						var newItem = new Item()
						{
							Description = itemDTO.Description,
							Price = itemDTO.Price,
							Units = itemDTO.Units,
							EventId = eventDomain.IdEvent
						};

						await App.ItemRepository.CreateAsync(newItem);

						newListItemEventDTO.Add(new ItemDTO()
						{
							Description = newItem.Description,
							IdItem = newItem.IdItem,
							Price = newItem.Price,
							Units = newItem.Units
						});
						
					}

					_listItemEventDTO = newListItemEventDTO;
					_listItemCheckBoxDTO = _listItemEventDTO.Select(a => new ItemCheckBoxDTO()
					{
						IdItem = a.IdItem,
						Description = a.Description,
						IsChecked = false
					}).ToList();
					AddPeopleEventView._listItemCheckBoxDTO = _listItemCheckBoxDTO;
				}

				break;

			case "WhoBought":
				if (eventDomain != null)
				{
					await App.PersonRepository.DeletePersonByEventIdAsync(eventDomain.IdEvent);
					var newListPersonDTO = new List<PersonDTO>();

					foreach (var personDTO in _listPersonDTO)
					{
						var newPerson = new Person()
						{
							IdPerson = personDTO.IdPerson,
							Name = personDTO.Name,
							ConsumeListItem = personDTO.ConsumeListItems.Select(b => new Item()
							{
								IdItem = b.IdItem,
								Description = b.Description
							}).ToList(),
							EventId = eventDomain.IdEvent
						};

						await App.PersonRepository.CreateAsync(newPerson);

						newListPersonDTO.Add(new PersonDTO()
						{
							IdPerson = newPerson.IdPerson,
							Name = newPerson.Name,
							ConsumeListItems = newPerson.ConsumeListItem.Select(b => new ItemDTO()
							{
								IdItem = b.IdItem,
								Description = b.Description
							}).ToList()
						});

					}

					_listPersonDTO = newListPersonDTO;
					foreach (var item in _listItemPersonBoughtDTO)
					{
						var personExists = newListPersonDTO.Any(p => p.Name.ToLower() == item.Person.ToLower());

						if (!personExists)
						{
							item.Person = string.Empty;
						}
					}
					AddPeopleEventView._listPersonDTO = _listPersonDTO;
					WhoBoughtView._listPersonDTO = _listPersonDTO;
					WhoBoughtView._listItemPersonBoughtDTO = _listItemPersonBoughtDTO;
					WhoBoughtView.InitializeStepFields();
					WhoBoughtView.InitializePicker();
				}
				break;

			case "Done":
				if (eventDomain != null)
				{
					await App.ItemPersonBoughtRepository.DeleteItemPersonBoughtByEventIdAsync(eventDomain.IdEvent);
					var newListItemPersonBoughtDTO = new List<ItemPersonBoughtDTO>();

					foreach (var itemPersonBoughtDTO in _listItemPersonBoughtDTO)
					{
						var newItemPersonBought = new ItemPersonBought()
						{
							Item = itemPersonBoughtDTO.Item,
							Person = itemPersonBoughtDTO.Person,
							EventId = eventDomain.IdEvent
						};

						await App.ItemPersonBoughtRepository.CreateAsync(newItemPersonBought);

						newListItemPersonBoughtDTO.Add(new ItemPersonBoughtDTO()
						{
							IdEvent = newItemPersonBought.EventId,
							Person = newItemPersonBought.Person,
							Item = newItemPersonBought.Item
						});

					}

					_listItemPersonBoughtDTO = newListItemPersonBoughtDTO;
					DoneEventView._eventDTO = _eventDTO;
					DoneEventView._listPersonDTO = _listPersonDTO;
					DoneEventView._listItemEventDTO = _listItemEventDTO;
					DoneEventView._listItemPersonBoughtDTO = _listItemPersonBoughtDTO;
					DoneEventView.InitializeStepFields();
				}
				break;
		}
	}

	private void LoadActiveView()
	{
		BasicInformationEventView.IsVisible = "BasicInformation" == ActiveStep;
		FoodAndDrinksEventView.IsVisible = "FoodAndDrinks" == ActiveStep;
		AddPeopleEventView.IsVisible = "AddPeople" == ActiveStep;
		WhoBoughtView.IsVisible = "WhoBought" == ActiveStep;
		DoneEventView.IsVisible = "Done" == ActiveStep;
	}

	private void StepsButtonsLayoutNextStep_Clicked(object sender, EventArgs e)
	{
		if (!ValidateEvent())
		{
			return;
		}

		LoadNextProgress();
		switch (ActiveStep)
		{
			case "BasicInformation":
				ActiveStep = "FoodAndDrinks";
				break;
			case "FoodAndDrinks":
				ActiveStep = "AddPeople";
				break;
			case "AddPeople":
				ActiveStep = "WhoBought";
				break;
			case "WhoBought":
				ActiveStep = "Done";
				break;
		}

		LoadActiveButtons();
		LoadActiveStep();
		LoadActiveView();
		SaveStep();
	}

	private void StepsButtonsLayoutBackStep_Clicked(object sender, EventArgs e)
	{
		LoadBackProgress();

		switch (ActiveStep)
		{
			case "FoodAndDrinks":
				ActiveStep = "BasicInformation";
				break;
			case "AddPeople":
				ActiveStep = "FoodAndDrinks";
				AddPeopleEventView.CleanFields();
				break;
			case "WhoBought":
				ActiveStep = "AddPeople";
				break;
			case "Done":
				ActiveStep = "WhoBought";
				break;
		}

		LoadActiveButtons();
		LoadActiveStep();
		LoadActiveView();
	}

	private async void StepsButtonsLayoutReturn_Clicked(object sender, EventArgs e)
	{
		await Application.Current.MainPage.Navigation.PopAsync();
	}

	private void SummaryAndEndButtonsLayoutBackStep_Clicked(object sender, EventArgs e)
	{
		LoadBackProgress();

		switch (ActiveStep)
		{
			case "FoodAndDrinks":
				ActiveStep = "BasicInformation";
				break;
			case "AddPeople":
				ActiveStep = "FoodAndDrinks";
				AddPeopleEventView.CleanFields();
				break;
			case "WhoBought":
				ActiveStep = "AddPeople";
				break;
			case "Done":
				ActiveStep = "WhoBought";
				break;
		}

		LoadActiveButtons();
		LoadActiveStep();
		LoadActiveView();
	}

	private void SummaryAndEndButtonsLayoutWhatsApp_Clicked(object sender, EventArgs e)
	{
		 _whatsAppService.ShareSummary(_eventDTO, _listPersonDTO, _listItemEventDTO, _listItemPersonBoughtDTO);
	}

	private async void SummaryAndEndButtonsLayoutFinish_Clicked(object sender, EventArgs e)
	{
		await Application.Current.MainPage.Navigation.PopAsync();
	}

	private bool ValidateEvent()
	{
		switch (ActiveStep)
		{
			case "BasicInformation":
				if (string.IsNullOrEmpty(_eventDTO.Name)) 
				{
					DisplayAlert("Error", "The event name cannot be empty.", "OK");
					return false;
				}
				else if (string.IsNullOrEmpty(_eventDTO.Description))
				{
					DisplayAlert("Error", "The event description cannot be empty.", "OK");
					return false;
				}
				break;

			case "FoodAndDrinks":
				if (_listItemEventDTO.Count == 0)
				{
					DisplayAlert("Error", "You have not added any items.", "OK");
					return false;
				}
				break;
		}

		return true;
	}
}