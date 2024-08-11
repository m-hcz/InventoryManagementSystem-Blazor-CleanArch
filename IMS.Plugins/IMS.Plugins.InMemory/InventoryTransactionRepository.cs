using IMS.CoreBusiness;
using IMS.UseCases.PluginInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMS.Plugins.InMemory
{
	public class InventoryTransactionRepository : IInventoryTransactionRepository
	{
		public List<InventoryTransaction> _inventoryTransactions = new();

		public void PurchaseAsync(string poNumber, Inventory inventory, int quantity, string doneBy, double price)
		{
			_inventoryTransactions.Add(new InventoryTransaction
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
		}

		public void ProduceAsync(string productionNumber, Inventory inventory, int quantityToConsume, string doneBy, double price)
		{
			_inventoryTransactions.Add(new InventoryTransaction
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
		}

	}
}
