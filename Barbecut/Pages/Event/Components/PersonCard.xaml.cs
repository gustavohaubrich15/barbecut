using Barbecut.Domain;

namespace Barbecut.Pages.Event.Components;

public partial class PersonCard : ContentView
{
	public PersonCard()
	{
		InitializeComponent();
	}

	public event EventHandler? _editPersonEvent;

	public event EventHandler? _removePersonEvent;

	public static readonly BindableProperty _rowTextProperty =
			BindableProperty.Create(nameof(_rowText), typeof(int), typeof(PersonCard), defaultValue: default(int), propertyChanged: OnRowTextChanged);

	public static readonly BindableProperty _nameTextProperty =
			BindableProperty.Create(nameof(_nameText), typeof(string), typeof(PersonCard), defaultValue: default(string), propertyChanged: OnNameTextChanged);

	public static readonly BindableProperty _consumeItemCountTextProperty =
			BindableProperty.Create(nameof(_consumeItemCountText), typeof(string), typeof(PersonCard), defaultValue: default(string), propertyChanged: OnConsumeItemCountTextChanged);

	public int _rowText
	{
		get => (int)GetValue(_rowTextProperty);
		set => SetValue(_rowTextProperty, value);
	}

	public string _nameText
	{
		get => (string)GetValue(_nameTextProperty);
		set => SetValue(_nameTextProperty, value);
	}

	public string _consumeItemCountText
	{
		get => (string)GetValue(_consumeItemCountTextProperty);
		set => SetValue(_consumeItemCountTextProperty, value);
	}

	private static void OnRowTextChanged(BindableObject bindable, object oldValue, object newValue)
	{
	}

	private static void OnNameTextChanged(BindableObject bindable, object oldValue, object newValue)
	{
		var personCard = (PersonCard)bindable;
		personCard.Name.Text = (string)newValue;
	}

	private static void OnConsumeItemCountTextChanged(BindableObject bindable, object oldValue, object newValue)
	{
		var personCard = (PersonCard)bindable;
		personCard.ConsumeItemCount.Text = $"{(string)newValue} items";
	}

	private void DeletePerson_Clicked(object sender, EventArgs e)
	{
		_removePersonEvent?.Invoke(_rowText, e);
	}

	private void EditPerson_Clicked(object sender, EventArgs e)
	{
		_editPersonEvent?.Invoke(_rowText, e);
	}
}