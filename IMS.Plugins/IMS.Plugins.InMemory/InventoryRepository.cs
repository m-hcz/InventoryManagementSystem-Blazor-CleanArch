using IMS.CoreBusiness;
using IMS.UseCases.PluginInterfaces;

namespace IMS.Plugins.InMemory
{
	public class InventoryRepository : IInventoryRepository
	{
		private List<Inventory> _inventories;

		public InventoryRepository()
		{
			_inventories = new List<Inventory>()
			{
				new Inventory() { InventoryId=1, InventoryName= "Bike Seat", Quantity=10,Price=2 },
				new Inventory() { InventoryId=2, InventoryName= "Bike Body", Quantity=10,Price=15 },
				new Inventory() { InventoryId=3, InventoryName= "Bike Wheel", Quantity=20,Price=8 },
				new Inventory() { InventoryId=4, InventoryName= "Bike Pedals", Quantity=20,Price=1 }
			};
		}

		public Task AddInventoryAsync(Inventory inventory)
		{
			if (inventory == null)
				return Task.CompletedTask;

			if (_inventories.Any(_ => _.InventoryName.Equals(inventory.InventoryName, StringComparison.OrdinalIgnoreCase)))
				return Task.CompletedTask;

			int maxId = _inventories.Max(x => x.InventoryId);
			inventory.InventoryId = maxId + 1;

			_inventories.Add(inventory);
			return Task.CompletedTask;
		}

		public Task DeleteInventoryByIdAsync(int id)
		{
			var inv = _inventories.First(_ => _.InventoryId == id);

			if (inv is not null)
				_inventories.Remove(inv);

			return Task.CompletedTask;
		}

		public async Task<IEnumerable<Inventory>> GetInventoriesByNameAsync(string name)
		{
			if (string.IsNullOrEmpty(name))
				return await Task.FromResult(_inventories);
			return _inventories.Where(_ => _.InventoryName.Contains(name, StringComparison.OrdinalIgnoreCase));
		}

		public async Task<Inventory> GetInventoryByIdAsync(int id)
		{
			var inv = _inventories.First(_ => _.InventoryId == id);

			return await Task.FromResult(inv);
		}

		public Task UpdateInventoryAsync(Inventory inventory)
		{
			if (_inventories.Any(_ => _.InventoryId != inventory.InventoryId && _.InventoryName.Equals(inventory.InventoryName, StringComparison.OrdinalIgnoreCase)))
				return Task.CompletedTask;

			var inv = _inventories.First(_ => _.InventoryId == inventory.InventoryId);

			if (inv is not null)
			{
				inv.InventoryName = inventory.InventoryName;
				inv.Quantity = inventory.Quantity;
				inv.Price = inventory.Price;
			}

			return Task.CompletedTask;
		}
	}
}
