using IMS.CoreBusiness;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMS.UseCases.PluginInterfaces
{
	public interface IProductRepository
	{
		Task AddProductAsync(Product Product);
		Task<Product> GetProductByIdAsync(int id);
		Task<IEnumerable<Product>> GetProductsByNameAsync(string name);
		Task UpdateProductAsync(Product Product);
		Task DeleteProductByIdAsync(int id);
	}
}
