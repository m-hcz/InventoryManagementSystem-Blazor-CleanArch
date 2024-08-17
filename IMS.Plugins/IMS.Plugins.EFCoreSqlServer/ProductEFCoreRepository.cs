using IMS.CoreBusiness;
using IMS.UseCases.PluginInterfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMS.Plugins.EFCoreSqlServer
{
	public class ProductEFCoreRepository(IDbContextFactory<IMSContext> _contextFactory) : IProductRepository
	{
		public async Task AddProductAsync(Product product)
		{
			using var db = _contextFactory.CreateDbContext();
			db.Products?.Add(product);
			FlagInventoryUnchanged(product, db);

			await db.SaveChangesAsync();
		}

		public async Task DeleteProductByIdAsync(int id)
		{
			using var db = _contextFactory.CreateDbContext();

			var product = db.Products?.Find(id);

			if (product == null) return;

			db.Products?.Remove(product);

			await db.SaveChangesAsync();
		}

		public async Task<Product?> GetProductByIdAsync(int id)
		{
			using var db = _contextFactory.CreateDbContext();
			var product = await db.Products.Include(_ => _.ProductInventories).ThenInclude(_ => _.Inventory).FirstOrDefaultAsync(_ => _.Id == id);

			return product;
		}

		public async Task<IEnumerable<Product>> GetProductsByNameAsync(string name)
		{
			using var db = _contextFactory.CreateDbContext();
			return await db.Products.Where(_ => _.Name.ToLower().IndexOf(name.ToLower()) >= 0).ToListAsync();
		}

		public async Task UpdateProductAsync(Product product)
		{
			using var db = _contextFactory.CreateDbContext();
			var p = await db.Products.Include(_ => _.ProductInventories).FirstOrDefaultAsync(_ => _.Id == product.Id);

			if (p is not null)
			{
				p.Name = product.Name;
				p.Quantity = product.Quantity;
				p.Price = product.Price;
				p.ProductInventories = product.ProductInventories;
				FlagInventoryUnchanged(product, db);

				await db.SaveChangesAsync();
			}
		}

		private void FlagInventoryUnchanged(Product product, IMSContext db)
		{
			if (product?.ProductInventories != null && product.ProductInventories.Any())
			{
				foreach (var item in product.ProductInventories)
				{
					if (item.Inventory != null)
					{
						db.Entry(item.Inventory).State = EntityState.Unchanged;
					}
				}
			}
		}
	}
}
