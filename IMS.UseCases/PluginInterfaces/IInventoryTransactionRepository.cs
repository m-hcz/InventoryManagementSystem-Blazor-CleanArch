﻿using IMS.CoreBusiness;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMS.UseCases.PluginInterfaces
{
	public interface IInventoryTransactionRepository
	{
		void PurchaseAsync(string poNumber, Inventory inventory, int quantity, string doneBy, double price);
		void ProduceAsync(string productionNumber, Inventory inventory, int quantityToConsume, string doneBy, double price);
		Task<IEnumerable<InventoryTransaction>> GetInventoryTransactionsAsync(string inventoryName, DateTime? dateFrom, DateTime? toDate, InventoryTransactionType? transactionType);
	}
}
