using IMS.CoreBusiness;
using IMS.Plugins.EFCoreSqlServer;
using IMS.UseCases.PluginInterfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMS.Plugins.InMemory
{
	public class ProductTransactionEFCoreRepository(IDbContextFactory<IMSContext> _contextFactory, IInventoryTransactionRepository _inventoryTransactionRepository, IProductRepository _productRepository, IInventoryRepository _inventoryRepository) : IProductTransactionRepository
	{
		public async Task<IEnumerable<ProductTransaction>> GetProductTransactionsAsync(string productName, DateTime? dateFrom, DateTime? toDate, ProductTransactionType? transactionType)
		{
			using var db = _contextFactory.CreateDbContext();

			var query = from pt in db.ProductTransactions
						join prod in db.Products on pt.ProductId equals prod.Id
						where (string.IsNullOrEmpty(productName) || prod.Name.ToLower().IndexOf(productName.ToLower()) >= 0)
							&&
							(!dateFrom.HasValue || pt.TransactionDate >= dateFrom.Value.Date)
							&&
							(!toDate.HasValue || pt.TransactionDate <= toDate.Value.Date)
							&&
							(!transactionType.HasValue || pt.ActivityType == transactionType)
						select pt;

			return await query.Include(_=>_.Product).ToListAsync();
		}

		public async Task ProduceAsync(string productNumber, Product product, int quantity, string doneBy)
		{
			using var db = _contextFactory.CreateDbContext();

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
			db.ProductTransactions?.Add(new ProductTransaction
			{
				ProductionNumber = productNumber,
				ProductId = product.Id,
				QuantityBefore = product.Quantity,
				QuantityAfter = product.Quantity + quantity,
				ActivityType = ProductTransactionType.ProduceProduct,
				TransactionDate = DateTime.UtcNow,
				DoneBy = doneBy,
			});

			await db.SaveChangesAsync();
		}

		public async Task SellProductAsync(string salesOrderNumber, Product product, int quantity, double unitPrice, string doneBy)
		{
			using var db = _contextFactory.CreateDbContext();

			db.ProductTransactions?.Add(new ProductTransaction
			{
				SONumber = salesOrderNumber,
				ProductId = product.Id,
				QuantityBefore = product.Quantity,
				QuantityAfter = product.Quantity - quantity,
				ActivityType = ProductTransactionType.SellProduct,
				TransactionDate = DateTime.UtcNow,
				DoneBy = doneBy,
				UnitPrice = unitPrice
			});

			await db.SaveChangesAsync();
		}
	}
}
