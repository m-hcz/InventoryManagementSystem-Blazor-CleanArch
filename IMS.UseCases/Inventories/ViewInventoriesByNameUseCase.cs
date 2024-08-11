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
    public class ViewInventoriesByNameUseCase(IInventoryRepository _inventoryRepository) : IViewInventoriesByNameUseCase
	{
		public async Task<IEnumerable<Inventory>> ExecuteAsync(string name = "")
		{
			return await _inventoryRepository.GetInventoriesByNameAsync(name);
		}
	}
}
