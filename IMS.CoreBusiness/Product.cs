using IMS.CoreBusiness.Validations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMS.CoreBusiness
{
	public class Product
	{
		public int Id { get; set; }
		[Required]
		[StringLength(150)]
		public string Name { get; set; } = string.Empty;
		[Range(0, int.MaxValue, ErrorMessage = "Quantity must be greator or equal to 0!")]
		public int Quantity { get; set; }
		[Range(0, int.MaxValue, ErrorMessage = "Price must be greator or equal to 0!")]
		public double Price { get; set; }

		[Product_EnsurePriceIsGreaterThanInventoriesCost]
		public List<ProductInventory> ProductInventories { get; set; } = new();

		public void AddInventory(Inventory inventory)
		{
			if (!ProductInventories.Any(_ => _.Inventory is not null && _.Inventory.InventoryName.Equals(inventory.InventoryName, StringComparison.OrdinalIgnoreCase)))
				ProductInventories.Add(new ProductInventory
				{
					InventoryId = inventory.InventoryId,
					InventoryQuantity = 1,
					Inventory = inventory,
					ProductId = this.Id,
					Product = this
				});
		}

		public void RemoveInventory(ProductInventory productInventory)
		{
			ProductInventories?.Remove(productInventory);
		}

	}
}
