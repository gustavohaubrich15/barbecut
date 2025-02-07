namespace Barbecut.Pages;

public partial class HelpPage : ContentPage
{
	public HelpPage()
	{
		InitializeComponent();
		NavigationPage.SetHasNavigationBar(this, false);
	}

	private async void Button_Clicked(object sender, EventArgs e)
	{
		await Application.Current.MainPage.Navigation.PopAsync();
	}
}