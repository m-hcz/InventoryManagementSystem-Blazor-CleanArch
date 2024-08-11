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
    public class ProduceProductUseCase(IProductTransactionRepository _productTransactionRepository, IProductRepository _productRepository) : IProduceProductUseCase
	{
		public async Task ExecuteAsync(string productNumber, Product product, int quantity, string doneBy)
		{
			// add transaction record
			await _productTransactionRepository.ProduceAsync(productNumber, product, quantity, doneBy);
			// decrese the quantity of inventories

			// update quantity of product
			product.Quantity += quantity;
			await _productRepository.UpdateProductAsync(product);
		}
	}
}
