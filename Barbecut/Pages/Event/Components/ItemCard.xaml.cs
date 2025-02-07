using Barbecut.Domain;
using Barbecut.DTO;
using Barbecut.Pages.Event.Steps;
using System.Windows.Input;

namespace Barbecut.Pages.Event.Components;

public partial class ItemCard : ContentView
{
	public ItemCard()
	{
		InitializeComponent();
	}

	public event EventHandler? _removeItemEvent;

	public static readonly BindableProperty _rowTextProperty =
			BindableProperty.Create(nameof(_rowText), typeof(int), typeof(ItemCard), defaultValue: default(int), propertyChanged: OnRowTextChanged);

	public static readonly BindableProperty _descriptionTextProperty =
			BindableProperty.Create(nameof(_descriptionText), typeof(string), typeof(ItemCard), defaultValue: default(string), propertyChanged: OnDescriptionTextChanged);

	public static readonly BindableProperty _priceTextProperty =
			BindableProperty.Create(nameof(_priceText), typeof(string), typeof(ItemCard), defaultValue: default(string), propertyChanged: OnPriceTextChanged);

	public static readonly BindableProperty _unitTextProperty =
			BindableProperty.Create(nameof(_unitText), typeof(string), typeof(ItemCard), defaultValue: default(string), propertyChanged: OnUnitTextChanged);

	public int _rowText
	{
		get => (int)GetValue(_rowTextProperty);
		set => SetValue(_rowTextProperty, value);
	}

	public string _descriptionText
	{
		get => (string)GetValue(_descriptionTextProperty);
		set => SetValue(_descriptionTextProperty, value);
	}

	public string _priceText
	{
		get => (string)GetValue(_priceTextProperty);
		set => SetValue(_priceTextProperty, value);
	}

	public string _unitText
	{
		get => (string)GetValue(_unitTextProperty);
		set => SetValue(_unitTextProperty, value);
	}

	private static void OnRowTextChanged(BindableObject bindable, object oldValue, object newValue)
	{
		var itemCard = (ItemCard)bindable;
		itemCard.Row.Text = ((int)newValue).ToString();
	}

	private static void OnDescriptionTextChanged(BindableObject bindable, object oldValue, object newValue)
	{
		var itemCard = (ItemCard)bindable;
		itemCard.Description.Text = (string)newValue;
	}

	private static void OnPriceTextChanged(BindableObject bindable, object oldValue, object newValue)
	{
		var itemCard = (ItemCard)bindable;
		itemCard.Price.Text = $"R$ {(string)newValue} un";
	}

	private static void OnUnitTextChanged(BindableObject bindable, object oldValue, object newValue)
	{
		var itemCard = (ItemCard)bindable;
		itemCard.Units.Text = $"{(string)newValue} units";
	}

	private void DeleteItem_Clicked(object sender, EventArgs e)
	{
		_removeItemEvent?.Invoke(_rowText, e);
	}

}