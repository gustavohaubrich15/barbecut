using Barbecut.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Barbecut.Services
{
	public class WhatsAppService
	{
		public async void ShareSummary(EventDTO _eventDTO, List<PersonDTO> _listPersonDTO, List<ItemDTO> _listItemEventDTO, List<ItemPersonBoughtDTO> _listItemPersonBought)
		{
			string message = $"*{_eventDTO.Name}*\n\n";

			message += $"*Description:* {_eventDTO.Description}\n\n";
			message += $"*Place:* Rua dos andradas, 200, Centro, Campo Bom/RS\n\n";
			message += $"*Date:* {_eventDTO.Date.ToString("dd/MM/yyyy")}\n\n";
			message += $"*Started Time:* {_eventDTO.Time.Hours:D2}:{_eventDTO.Time.Minutes:D2}\n\n";

			message += $"*List of confirmed people:*\n";
			foreach (var person in _listPersonDTO)
			{
				message += $"• {person.Name}\n";
			}

			message += "\n";

			message += $"*List of food and drinks:*\n";
			foreach (var item in _listItemEventDTO)
			{
				message += $"• {item.Description.ToUpper()}; Price - R$ {item.Price.ToString("F2")}; Units - {item.Units}\n";
			}

			message += "\n";

			message += $"*List of who bought items:*\n";
			foreach (var itemPersonBought in _listItemPersonBought)
			{
				message += $"• {itemPersonBought.Person} - {itemPersonBought.Item}\n";
			}

			string whatsappUrl = $"https://wa.me/?text={Uri.EscapeDataString(message)}";

			try
			{
				await Launcher.Default.OpenAsync(whatsappUrl);
			}
			catch 
			{
				await Application.Current.MainPage.DisplayAlert("Erro", "Não foi possível compartilhar no WhatsApp.", "OK");
			}
		}

		public async void ShareConsumption(EventDTO _eventDTO, List<PayPersonDTO> _listPayPersonDTO, List<OperationPayDTO> _listOperationPayDTO)
		{
			string message = $"*EVENT CONSUMPTION SUMMARY :* {_eventDTO.Name}\n\n";
			message += $"*Date:* {_eventDTO.Date.ToString("dd/MM/yyyy")}\n\n";

			foreach (var payPerson in _listPayPersonDTO)
			{
				message += $"*Item:* {payPerson.Item} - *Who Bought:* {payPerson.NamePerson} - *Amount Paid:* R$ {payPerson.ItemTotal.ToString("F2")}\n\n";
				message += $"Value per person: R$ *{payPerson.PriceForPerson.ToString("F2")}*\n\n";
				message += $"*Consume List Item*\n\n";
				message += $"*---------------------------------------------*\n\n";
				message += $"• {string.Join(", ", payPerson.ListConsumePersonItem)}\n\n";
				message += $"*---------------------------------------------*\n\n\n\n";
			}

			message += "\n";

			message += $"*Operations*\n";
			foreach (var operation in _listOperationPayDTO)
			{
				message += $"• {operation.ReceivePayPerson.ToUpper()} *pay to* {operation.PayPerson.ToUpper()} value of R$ {operation.Value.ToString("F2")}\n";
			}

			string whatsappUrl = $"https://wa.me/?text={Uri.EscapeDataString(message)}";

			try
			{
				await Launcher.Default.OpenAsync(whatsappUrl);
			}
			catch
			{
				await Application.Current.MainPage.DisplayAlert("Erro", "Não foi possível compartilhar no WhatsApp.", "OK");
			}
		}

	}
}
