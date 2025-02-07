using Barbecut.Domain;
using System.Reflection;
using Newtonsoft.Json;
using SQLite;
using SQLiteNetExtensions.Attributes;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;

namespace Barbecut.Repositories
{
    public class Repository<T> where T : new()
	{
        private readonly SQLiteAsyncConnection _database;

        public Repository()
        {
            var dbDirectory = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            var dbPath = Path.Combine(dbDirectory, "barbecutRepository.db");
            _database = new SQLiteAsyncConnection(dbPath);
			_database.CreateTableAsync<T>().Wait();
		}

		public async Task<int> CreateAsync(T entity)
		{
			var textBlobProperties = entity.GetType()
										   .GetProperties()
										   .Where(p => Attribute.IsDefined(p, typeof(TextBlobAttribute)));

			foreach (var property in textBlobProperties)
			{
				if (property.PropertyType == typeof(List<Item>))
				{
					var listValue = property.GetValue(entity) as List<Item>;
					var serializedValue = JsonConvert.SerializeObject(listValue);

					var blobProperty = entity.GetType().GetProperty(property.Name + "Blob");
					if (blobProperty != null)
					{
						blobProperty.SetValue(entity, serializedValue);
					}
				}
			}

			return await _database.InsertAsync(entity);
		}

		public async Task<T> GetByIdAsync(int id)
		{
			return await _database.FindAsync<T>(id);
		}
		public async Task<List<T>> GetAllAsync()
		{
			return await _database.Table<T>().ToListAsync();
		}
		
		public Task<int> UpdateAsync(T entity)
		{
			return _database.UpdateAsync(entity);
		}

		public async Task<int> DeleteAsync(T entity)
		{
			return await _database.DeleteAsync(entity);
		}

		public async Task<int> DeleteByIdAsync(int id)
		{
			var entity = await GetByIdAsync(id);
			if (entity != null)
			{
				return await _database.DeleteAsync(entity);
			}
			return 0;
		}

		public async Task DeleteItemsByEventIdAsync(int eventId)
		{
			var items = await _database.Table<Item>().Where(i => i.EventId == eventId).ToListAsync();
			foreach (var item in items)
			{
				await _database.DeleteAsync(item);
			}
		}

		public async Task DeletePersonByEventIdAsync(int eventId)
		{
			var listPerson = await _database.Table<Person>().Where(i => i.EventId == eventId).ToListAsync();
			foreach (var person in listPerson)
			{
				await _database.DeleteAsync(person);
			}
		}

		public async Task DeleteItemPersonBoughtByEventIdAsync(int eventId)
		{
			var listItemPersonBought = await _database.Table<ItemPersonBought>().Where(i => i.EventId == eventId).ToListAsync();
			foreach (var itemPersonBought in listItemPersonBought)
			{
				await _database.DeleteAsync(itemPersonBought);
			}
		}

		public async Task<List<Person>> GetPersonsByEventIdAsync(int eventId)
		{
			return await _database.Table<Person>().Where(i => i.EventId == eventId).ToListAsync();
		}

		public async Task<List<Item>> GetItemsByEventIdAsync(int eventId)
		{
			return await _database.Table<Item>().Where(i => i.EventId == eventId).ToListAsync();
		}

		public async Task<List<ItemPersonBought>> GetItemPersonBoughtByEventIdAsync(int eventId)
		{
			return await _database.Table<ItemPersonBought>().Where(i => i.EventId == eventId).ToListAsync();
		}

	}
}
