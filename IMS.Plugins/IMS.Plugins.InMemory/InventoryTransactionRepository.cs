﻿using IMS.CoreBusiness;
using IMS.UseCases.PluginInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMS.Plugins.InMemory
{
	public class InventoryTransactionRepository(IInventoryRepository _inventoryRepository) : IInventoryTransactionRepository
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

		public async Task<IEnumerable<InventoryTransaction>> GetInventoryTransactionsAsync(string inventoryName, DateTime? dateFrom, DateTime? toDate, InventoryTransactionType? transactionType)
		{
			var inventories = (await _inventoryRepository.GetInventoriesByNameAsync(string.Empty)).ToList();

			var query = from it in _inventoryTransactions
						join inv in inventories on it.InventoryId equals inv.InventoryId
						where (string.IsNullOrEmpty(inventoryName) || inv.InventoryName.ToLower().IndexOf(inventoryName.ToLower()) >= 0)
							&&
							(!dateFrom.HasValue || it.TransactionDate >= dateFrom.Value.Date)
							&&
							(!toDate.HasValue || it.TransactionDate <= toDate.Value.Date)
							&&
							(!transactionType.HasValue || it.ActivityType == transactionType)
						select new InventoryTransaction
						{
							Inventory = inv,
							TransactionId = it.TransactionId,
							PONumber = it.PONumber,
							InventoryId = it.InventoryId,
							QuantityBefore = it.QuantityBefore,
							QuantityAfter = it.QuantityAfter,
							ActivityType = it.ActivityType,
							TransactionDate = it.TransactionDate,
							DoneBy = it.DoneBy,
							UnitPrice = it.UnitPrice,
						};

			return query;
		}
	}
}
