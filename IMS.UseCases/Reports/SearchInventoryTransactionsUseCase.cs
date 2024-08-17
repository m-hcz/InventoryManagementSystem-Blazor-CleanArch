using IMS.CoreBusiness;
using IMS.UseCases.PluginInterfaces;
using IMS.UseCases.Reports.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMS.UseCases.Reports
{
	public class SearchInventoryTransactionsUseCase(IInventoryTransactionRepository _inventoryTransactionRepository) : ISearchInventoryTransactionsUseCase
	{
		public async Task<IEnumerable<InventoryTransaction>> ExecuteAsync(string inventoryName, DateTime? dateFrom, DateTime? toDate, InventoryTransactionType? transactionType)
		{
			if (toDate.HasValue) toDate = toDate.Value.AddDays(1);

			return await _inventoryTransactionRepository.GetInventoryTransactionsAsync(inventoryName, dateFrom, toDate, transactionType);
		}
	}
}
