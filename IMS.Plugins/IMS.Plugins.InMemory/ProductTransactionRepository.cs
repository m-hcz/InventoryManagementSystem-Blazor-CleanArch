﻿using IMS.CoreBusiness;
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

		public async Task<IEnumerable<ProductTransaction>> GetProductTransactionsAsync(string productName, DateTime? dateFrom, DateTime? toDate, ProductTransactionType? transactionType)
		{
			var products = (await _productRepository.GetProductsByNameAsync(string.Empty)).ToList();

			var query = from it in _productTransactions
						join prod in products on it.ProductId equals prod.Id
						where (string.IsNullOrEmpty(productName) || prod.Name.ToLower().IndexOf(productName.ToLower()) >= 0)
							&&
							(!dateFrom.HasValue || it.TransactionDate >= dateFrom.Value.Date)
							&&
							(!toDate.HasValue || it.TransactionDate <= toDate.Value.Date)
							&&
							(!transactionType.HasValue || it.ActivityType == transactionType)
						select new ProductTransaction
						{
							Product = prod,
							TransactionId = it.TransactionId,
							ProductionNumber = it.ProductionNumber,
							SONumber = it.SONumber,
							ProductId = it.ProductId,
							QuantityBefore = it.QuantityBefore,
							QuantityAfter = it.QuantityAfter,
							ActivityType = it.ActivityType,
							TransactionDate = it.TransactionDate,
							DoneBy = it.DoneBy,
							UnitPrice = it.UnitPrice,
						};

			return query;
		}

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

		public Task SellProductAsync(string salesOrderNumber, Product product, int quantity, double unitPrice, string doneBy)
		{
			_productTransactions.Add(new ProductTransaction
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

			return Task.CompletedTask;
		}
	}
}
