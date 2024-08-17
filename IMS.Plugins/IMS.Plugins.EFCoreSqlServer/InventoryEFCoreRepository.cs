using IMS.CoreBusiness;
using IMS.UseCases.PluginInterfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMS.Plugins.EFCoreSqlServer
{
	public class InventoryEFCoreRepository(IDbContextFactory<IMSContext> _contextFactory) : IInventoryRepository
	{
		public async Task AddInventoryAsync(Inventory inventory)
		{
			using var db = _contextFactory.CreateDbContext();
			db.Inventories?.Add(inventory);

			await db.SaveChangesAsync();
		}

		public async Task DeleteInventoryByIdAsync(int id)
		{
			using var db = _contextFactory.CreateDbContext();

			var inventory = db.Inventories?.Find(id);

			if (inventory == null) return;

			db.Inventories?.Remove(inventory);

			await db.SaveChangesAsync();
		}

		public async Task<IEnumerable<Inventory>> GetInventoriesByNameAsync(string name)
		{
			using var db = _contextFactory.CreateDbContext();
			return await db.Inventories.Where(_ => _.InventoryName.ToLower().IndexOf(name.ToLower()) >= 0).ToListAsync();
		}

		public async Task<Inventory> GetInventoryByIdAsync(int id)
		{
			using var db = _contextFactory.CreateDbContext();
			var inventory = await db.Inventories.FindAsync(id);

			if (inventory == null) 
				return new Inventory();

			return inventory;
		}

		public async Task UpdateInventoryAsync(Inventory inventory)
		{
			using var db = _contextFactory.CreateDbContext();
			var inv = await db.Inventories.FindAsync(inventory.InventoryId);

			if (inv is not null)
			{
				inv.InventoryName = inventory.InventoryName;
				inv.Quantity = inventory.Quantity;
				inv.Price = inventory.Price;

				await db.SaveChangesAsync();
			}
		}
	}
}
