using IMS.CoreBusiness;
using IMS.UseCases.PluginInterfaces;
using IMS.UseCases.Products.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMS.UseCases.Products
{
    public class ViewProductsByNameUseCase(IProductRepository _productsRepository) : IViewProductsByNameUseCase
    {
        public async Task<IEnumerable<Product>> ExecuteAsync(string name = "")
        {
            return await _productsRepository.GetProductsByNameAsync(name);
        }
    }
}
