using IMS.CoreBusiness;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMS.UseCases.PluginInterfaces
{
	public interface IInventoryRepository
	{
		Task AddInventoryAsync(Inventory inventory);
		Task<Inventory> GetInventoryByIdAsync(int id);
		Task<IEnumerable<Inventory>> GetInventoriesByNameAsync(string name);
		Task UpdateInventoryAsync(Inventory inventory);
		Task DeleteInventoryByIdAsync(int id);
	}
}
