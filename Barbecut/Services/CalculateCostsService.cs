using Barbecut.Domain;
using Barbecut.DTO;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Barbecut.Services
{
	public class CalculateCostsService
	{
		public List<PayPersonDTO> _listPayPersonDTO { get; set; } = new List<PayPersonDTO>();
		public List<OperationPayDTO> _listOperationPayDTO { get; set; } = new List<OperationPayDTO>();
		public async Task Calculate(EventDTO eventToShareCosts)
		{
			_listPayPersonDTO.Clear();
			_listOperationPayDTO.Clear();
			var listPerson = await App.PersonRepository.GetPersonsByEventIdAsync(eventToShareCosts.IdEvent);
			var listPersonDTO = listPerson.Select(a => new PersonDTO()
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
			}).ToList();
			var listItemPersonBought = await App.ItemPersonBoughtRepository.GetItemPersonBoughtByEventIdAsync(eventToShareCosts.IdEvent);
			var listItem = await App.ItemRepository.GetItemsByEventIdAsync(eventToShareCosts.IdEvent);

			foreach (var item in listItem)
			{
				var personWhoBought = listItemPersonBought.Where(a => a.Item.ToLower() == item.Description.ToLower()).FirstOrDefault();
				var listConsumePersonItem = listPersonDTO.Where(a => a.ConsumeListItems.Where(b => b.Description.ToLower() == item.Description.ToLower()).Any()).ToList();

				_listPayPersonDTO.Add(new PayPersonDTO()
				{
					ItemTotal = item.Price * item.Units,
					Item = item.Description,
					NamePerson = personWhoBought?.Person ?? string.Empty,
					ListConsumePersonItem = listConsumePersonItem.Select(a => a.Name).ToList()
				});

			}

			foreach (var person in listPersonDTO)
			{
				var listConsume = _listPayPersonDTO.Where(a => a.ListConsumePersonItem.Where(b =>b.ToLower() != a.NamePerson.ToLower() &&  b.ToLower() == person.Name.ToLower()).Any()).ToList();

				var groupConsume = listConsume.SelectMany(a => a.ListConsumePersonItem
											  .Where(b => b.ToLower() == person.Name.ToLower())
													.Select(b => new
													{
														PayPerson = a.NamePerson,
														Value = a.PriceForPerson
													}))
												.GroupBy(x => x.PayPerson)
												.Select(g => new OperationPayDTO
												{
													PayPerson = g.Key,
													ReceivePayPerson = person.Name,
													Value = g.Sum(x => x.Value)
												}).ToList();
				if (groupConsume.Any())
				{
					_listOperationPayDTO.AddRange(groupConsume);
				}
			}
		}
	}
}
