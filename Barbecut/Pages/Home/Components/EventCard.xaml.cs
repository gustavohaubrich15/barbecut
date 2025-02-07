namespace Barbecut.Pages.Home.Components;

public partial class EventCard : ContentView
{
	public EventCard()
	{
		InitializeComponent();
	}

	public event EventHandler? _shareCostsEvent;
	public event EventHandler? _editEvent;
	public event EventHandler? _removeEvent;

	public static readonly BindableProperty _idTextProperty =
			BindableProperty.Create(nameof(_idText), typeof(int), typeof(EventCard), defaultValue: default(int));

	public static readonly BindableProperty _nameTextProperty =
			BindableProperty.Create(nameof(_nameText), typeof(string), typeof(EventCard), defaultValue: default(string), propertyChanged: OnNameTextChanged);

	public static readonly BindableProperty _statusTextProperty =
			BindableProperty.Create(nameof(_statusText), typeof(string), typeof(EventCard), defaultValue: default(string), propertyChanged: OnStatusTextChanged);

	public int _idText
	{
		get => (int)GetValue(_idTextProperty);
		set => SetValue(_idTextProperty, value);
	}

	public string _nameText
	{
		get => (string)GetValue(_nameTextProperty);
		set => SetValue(_nameTextProperty, value);
	}

	public string _statusText
	{
		get => (string)GetValue(_statusTextProperty);
		set => SetValue(_statusTextProperty, value);
	}

	private static void OnNameTextChanged(BindableObject bindable, object oldValue, object newValue)
	{
		var eventCard = (EventCard)bindable;
		eventCard.Name.Text = (string)newValue;
	}

	private static void OnStatusTextChanged(BindableObject bindable, object oldValue, object newValue)
	{
		var eventCard = (EventCard)bindable;
		eventCard.Status.Text = (string)newValue;
		eventCard.StatusCircle.Fill = eventCard.Status.Text == "Active event" ? new SolidColorBrush(Color.Parse("#0BD637")) : new SolidColorBrush(Color.Parse("#C70E16"));
	}

	private void DeleteEvent_Clicked(object sender, EventArgs e)
	{
		_removeEvent?.Invoke(_idText, e);
	}

	private void EditEvent_Clicked(object sender, EventArgs e)
	{
		_editEvent?.Invoke(_idText, e);
	}

	private void ShareCostsEvent_Clicked(object sender, EventArgs e)
	{
		_shareCostsEvent?.Invoke(_idText, e);
	}
}