using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using Microsoft.Maui.Controls.Hosting;
using Barbecut.Pages.Events;
using Barbecut.Pages.Event.Interface;
using CommunityToolkit.Maui;

namespace Barbecut
{
	public static class MauiProgram
	{
		public static MauiApp CreateMauiApp()
		{
			var builder = MauiApp.CreateBuilder();
			builder
				.UseMauiApp<App>()
				.UseMauiCommunityToolkit()
				.ConfigureFonts(fonts =>
				{
					fonts.AddFont("Inter-Regular.ttf", "InterRegular");
				});

#if DEBUG
			builder.Logging.AddDebug();
#endif
			
			return builder.Build();
		}
	}
}
