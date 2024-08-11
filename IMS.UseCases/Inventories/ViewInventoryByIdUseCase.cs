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
    public class ViewInventoryByIdUseCase(IInventoryRepository _inventoryRepository) : IViewInventoryByIdUseCase
	{
		public async Task<Inventory> ExecuteAsync(int id)
		{
			return await _inventoryRepository.GetInventoryByIdAsync(id);
		}
	}
}
