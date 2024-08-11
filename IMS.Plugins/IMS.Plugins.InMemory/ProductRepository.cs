using IMS.CoreBusiness;
using IMS.UseCases.PluginInterfaces;

namespace IMS.Plugins.InMemory
{
	public class ProductRepository : IProductRepository
	{
		private List<Product> _products;

		public ProductRepository()
		{
			_products = new List<Product>()
			{
				new Product() { Id=1, Name= "Bike", Quantity=10,Price=150 },
				new Product() { Id=2, Name= "Car", Quantity=10,Price=2000 },
			};
		}

		public Task AddProductAsync(Product product)
		{
			if (product == null)
				return Task.CompletedTask;

			if (_products.Any(_ => _.Name.Equals(product.Name, StringComparison.OrdinalIgnoreCase)))
				return Task.CompletedTask;

			int maxId = _products.Max(x => x.Id);
			product.Id = maxId + 1;

			_products.Add(product);
			return Task.CompletedTask;
		}

		public Task DeleteProductByIdAsync(int id)
		{
			var inv = _products.First(_ => _.Id == id);

			if (inv is not null)
				_products.Remove(inv);

			return Task.CompletedTask;
		}

		public async Task<IEnumerable<Product>> GetProductsByNameAsync(string name)
		{
			if (string.IsNullOrEmpty(name))
				return await Task.FromResult(_products);
			return _products.Where(_ => _.Name.Contains(name, StringComparison.OrdinalIgnoreCase));
		}

		public async Task<Product?> GetProductByIdAsync(int id)
		{
			var prod = _products.FirstOrDefault(_ => _.Id == id);
			var newProd = new Product();

			if (prod is not null)
			{
				newProd.Id = prod.Id;
				newProd.Name = prod.Name;
				newProd.Price = prod.Price;
				newProd.Quantity = prod.Quantity;
				newProd.ProductInventories = new List<ProductInventory>();

				if(prod.ProductInventories != null)
					foreach (var prodInv in prod.ProductInventories)
					{
						var newProdInv = new ProductInventory
						{
							InventoryId = prodInv.InventoryId,
							ProductId = prodInv.ProductId,
							Product = prod,
							Inventory = new Inventory(),
							InventoryQuantity=prodInv.InventoryQuantity,
						};

						if (prodInv.Inventory != null)
						{
							newProdInv.Inventory.InventoryId = prodInv.Inventory.InventoryId;
							newProdInv.Inventory.InventoryName = prodInv.Inventory.InventoryName;
							newProdInv.Inventory.Price = prodInv.Inventory.Price;
							newProdInv.Inventory.Quantity = prodInv.Inventory.Quantity;
						}

						newProd.ProductInventories.Add(newProdInv);
					}
			}

			return await Task.FromResult(newProd);
		}

		public Task UpdateProductAsync(Product product)
		{
			if (_products.Any(_ => _.Id != product.Id && _.Name.Equals(product.Name, StringComparison.OrdinalIgnoreCase)))
				return Task.CompletedTask;

			var p = _products.FirstOrDefault(_ => _.Id == product.Id);

			if (p is not null)
			{
				p.Name = product.Name;
				p.Quantity = product.Quantity;
				p.Price = product.Price;
				p.ProductInventories=product.ProductInventories;
			}

			return Task.CompletedTask;
		}
	}
}
