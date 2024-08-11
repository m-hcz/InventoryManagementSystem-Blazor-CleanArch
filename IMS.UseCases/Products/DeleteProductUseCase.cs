using IMS.CoreBusiness;
using IMS.UseCases.Inventories.Interfaces;
using IMS.UseCases.PluginInterfaces;
using IMS.UseCases.Products.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMS.UseCases.Products
{
    public class DeleteProductUseCase(IProductRepository _productsRepository) : IDeleteProductUseCase
	{
        public async Task ExecuteAsync(int id)
        {
            await _productsRepository.DeleteProductByIdAsync(id);
        }
    }
}
