using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Barbecut.Utils
{
	public static class Alert
	{
		public static async void ShowAlert(string typeError, string message)
		{
			await Application.Current.MainPage.DisplayAlert(typeError, message, "OK");
		}
	}
}
