using Barbecut.DTO;
using Barbecut.Pages.Event.Interface;
using System.ComponentModel;
using System.Runtime.CompilerServices;
namespace Barbecut.Pages.Event.Steps;
public partial class BasicInformationEvent : ContentView, IEventStep
{
	public EventDTO _eventDTO { get; set; } = new EventDTO();

	public BasicInformationEvent()
	{
		InitializeComponent();
	}

	public void InitializeStepFields()
	{
		Name.Text = _eventDTO.Name;
		Description.Text = _eventDTO.Description;
		Location.Text = _eventDTO.Location;
		BorderDatePicker.Date = _eventDTO.Date;
		BorderTimePicker.Time = _eventDTO.Time;
	}

	private void Name_TextChanged(object sender, TextChangedEventArgs e)
	{
		_eventDTO.Name = Name.Text;
	}

	private void Description_TextChanged(object sender, TextChangedEventArgs e)
	{
		_eventDTO.Description = Description.Text;
	}

	private void BorderDatePicker_DateSelected(object sender, DateChangedEventArgs e)
	{
		_eventDTO.Date = e.NewDate;
	}

	private void Location_TextChanged(object sender, TextChangedEventArgs e)
	{
		_eventDTO.Location = Location.Text;
	}

	private void BorderTimePicker_PropertyChanged(object sender, PropertyChangedEventArgs e)
	{
		_eventDTO.Time = BorderTimePicker.Time;
	}
}