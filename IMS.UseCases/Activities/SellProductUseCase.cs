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
    public class SellProductUseCase(IProductTransactionRepository _productTransactionRepository, IProductRepository _productRepository) : ISellProductUseCase
	{
		public async Task ExecuteAsync(string salesOrderNumber, Product product, int quantity,double unitPrice, string doneBy)
		{
			await _productTransactionRepository.SellProductAsync(salesOrderNumber, product, quantity,unitPrice, doneBy);

			product.Quantity -= quantity;
			_productRepository.UpdateProductAsync(product);
		}
	}
}
