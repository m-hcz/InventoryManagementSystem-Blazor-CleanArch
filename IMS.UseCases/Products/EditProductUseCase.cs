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
    public class EditProductUseCase(IProductRepository _productsRepository) : IEditProductUseCase
    {
        public async Task ExecuteAsync(Product product)
        {
            await _productsRepository.UpdateProductAsync(product);
        }
    }
}
