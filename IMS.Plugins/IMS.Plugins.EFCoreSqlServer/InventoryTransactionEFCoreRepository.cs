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
	public class InventoryTransactionEFCoreRepository(IDbContextFactory<IMSContext> _contextFactory) : IInventoryTransactionRepository
	{
		public async void PurchaseAsync(string poNumber, Inventory inventory, int quantity, string doneBy, double price)
		{
			using var db = _contextFactory.CreateDbContext();

			db.InventoryTransactions?.Add(new InventoryTransaction
			{
				PONumber = poNumber,
				InventoryId = inventory.InventoryId,
				QuantityBefore = inventory.Quantity,
				QuantityAfter = inventory.Quantity + quantity,
				ActivityType = InventoryTransactionType.PurchaseInventory,
				TransactionDate = DateTime.UtcNow,
				DoneBy = doneBy,
				UnitPrice = price
			});

			await db.SaveChangesAsync();
		}

		public async void ProduceAsync(string productionNumber, Inventory inventory, int quantityToConsume, string doneBy, double price)
		{
			using var db = _contextFactory.CreateDbContext();

			db.InventoryTransactions?.Add(new InventoryTransaction
			{
				ProductionNumber = productionNumber,
				InventoryId = inventory.InventoryId,
				QuantityBefore = inventory.Quantity,
				QuantityAfter = inventory.Quantity - quantityToConsume,
				ActivityType = InventoryTransactionType.ProduceProduct,
				TransactionDate = DateTime.UtcNow,
				DoneBy = doneBy,
				UnitPrice = price
			});

			await db.SaveChangesAsync();
		}

		public async Task<IEnumerable<InventoryTransaction>> GetInventoryTransactionsAsync(string inventoryName, DateTime? dateFrom, DateTime? toDate, InventoryTransactionType? transactionType)
		{
			using var db = _contextFactory.CreateDbContext();

			var query = from it in db.InventoryTransactions
						join inv in db.Inventories on it.InventoryId equals inv.InventoryId
						where (string.IsNullOrEmpty(inventoryName) || inv.InventoryName.ToLower().IndexOf(inventoryName.ToLower()) >= 0)
							&&
							(!dateFrom.HasValue || it.TransactionDate >= dateFrom.Value.Date)
							&&
							(!toDate.HasValue || it.TransactionDate <= toDate.Value.Date)
							&&
							(!transactionType.HasValue || it.ActivityType == transactionType)
						select it;

			return await query.Include(_=>_.Inventory).ToListAsync();
		}
	}
}
