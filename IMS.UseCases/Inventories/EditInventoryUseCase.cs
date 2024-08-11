using IMS.CoreBusiness;
using IMS.UseCases.Inventories.Interfaces;
using IMS.UseCases.PluginInterfaces;
using IMS.UseCases.Products.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMS.UseCases.Inventories
{
    public class EditInventoryUseCase(IInventoryRepository _inventoryRepository) : IEditInventoryUseCase
	{
		public async Task ExecuteAsync(Inventory inventory)
		{
			await _inventoryRepository.UpdateInventoryAsync(inventory);
		}
	}
}
