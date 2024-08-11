using IMS.CoreBusiness;
using IMS.UseCases.PluginInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMS.Plugins.InMemory
{
	public class ProductTransactionRepository(IInventoryTransactionRepository _inventoryTransactionRepository, IProductRepository _productRepository, IInventoryRepository _inventoryRepository) : IProductTransactionRepository
	{
		List<ProductTransaction> _productTransactions = new();

		public async Task ProduceAsync(string productNumber, Product product, int quantity, string doneBy)
		{
			var prod = await _productRepository.GetProductByIdAsync(product.Id);

			if (prod != null)
			{
				foreach (var pi in prod.ProductInventories)
				{
					if (pi.Inventory != null)
					{
						// add inventory transaction
						_inventoryTransactionRepository.PurchaseAsync(productNumber, pi.Inventory, pi.InventoryQuantity * quantity, doneBy, -1);

						// decrese the inventories
						var inv = await _inventoryRepository.GetInventoryByIdAsync(pi.InventoryId);
						inv.Quantity -= pi.InventoryQuantity * quantity;
						await _inventoryRepository.UpdateInventoryAsync(inv);
					}
				}
			}

			// add product transaction
			_productTransactions.Add(new ProductTransaction
			{
				ProductionNumber = productNumber,
				ProductId = product.Id,
				QuantityBefore = product.Quantity,
				QuantityAfter = product.Quantity + quantity,
				ActivityType = ProductTransactionType.ProduceProduct,
				TransactionDate = DateTime.UtcNow,
				DoneBy = doneBy,
			});
		}
	}
}
