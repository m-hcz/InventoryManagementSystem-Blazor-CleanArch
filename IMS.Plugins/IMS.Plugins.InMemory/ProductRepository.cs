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

		public async Task<Product> GetProductByIdAsync(int id)
		{
			var inv = _products.First(_ => _.Id == id);

			return await Task.FromResult(inv);
		}

		public Task UpdateProductAsync(Product product)
		{
			if (_products.Any(_ => _.Id != product.Id && _.Name.Equals(product.Name, StringComparison.OrdinalIgnoreCase)))
				return Task.CompletedTask;

			var p = _products.First(_ => _.Id == product.Id);

			if (p is not null)
			{
				p.Name = product.Name;
				p.Quantity = product.Quantity;
				p.Price = product.Price;
			}

			return Task.CompletedTask;
		}
	}
}
