using IMS.CoreBusiness;
using IMS.UseCases.Activities.Interfaces;
using IMS.UseCases.PluginInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMS.UseCases.Activities
{
	public class PurchaseInventoryUseCase(IInventoryTransactionRepository _inventoryTransactionRepository, IInventoryRepository _inventoryRepository) : IPurchaseInventoryUseCase
	{
		public async Task ExecuteAsync(string poNumber, Inventory inventory, int quantity, string doneBy)
		{
			// insert a record in the transaction table
			_inventoryTransactionRepository.PurchaseAsync(poNumber, inventory, quantity, doneBy, inventory.Price);

			// incrase the quantity
			inventory.Quantity += quantity;

			await _inventoryRepository.UpdateInventoryAsync(inventory);
		}
	}
}
