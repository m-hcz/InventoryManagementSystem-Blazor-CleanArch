using IMS.CoreBusiness;
using Microsoft.EntityFrameworkCore;

namespace IMS.Plugins.EFCoreSqlServer
{
	public class IMSContext : DbContext
	{
        public IMSContext(DbContextOptions<IMSContext> options): base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			// relationships
			modelBuilder.Entity<ProductInventory>()
				.HasKey(pi => new { pi.ProductId, pi.InventoryId });

			modelBuilder.Entity<ProductInventory>()
				.HasOne(pi => pi.Product)
				.WithMany(p => p.ProductInventories)
				.HasForeignKey(pi => pi.ProductId);

			modelBuilder.Entity<ProductInventory>()
				.HasOne(pi => pi.Inventory)
				.WithMany(p => p.ProductInventories)
				.HasForeignKey(pi => pi.InventoryId);

			// seeding data
			modelBuilder.Entity<Inventory>().HasData(
					new Inventory() { InventoryId = 1, InventoryName = "Bike Seat", Quantity = 10, Price = 2 },
					new Inventory() { InventoryId = 2, InventoryName = "Bike Body", Quantity = 10, Price = 15 },
					new Inventory() { InventoryId = 3, InventoryName = "Bike Wheel", Quantity = 20, Price = 8 },
					new Inventory() { InventoryId = 4, InventoryName = "Bike Pedals", Quantity = 20, Price = 1 }
				);

			modelBuilder.Entity<Product>().HasData(
					new Product() { Id = 1, Name = "Bike", Quantity = 10, Price = 150 },
					new Product() { Id = 2, Name = "Car", Quantity = 10, Price = 25000 }
				);

			modelBuilder.Entity<ProductInventory>().HasData(
					new ProductInventory { ProductId = 1, InventoryId = 1, InventoryQuantity = 1}, // seat
					new ProductInventory { ProductId = 1, InventoryId = 2, InventoryQuantity = 1}, // body
					new ProductInventory { ProductId = 1, InventoryId = 3, InventoryQuantity = 2}, // wheels
					new ProductInventory { ProductId = 1, InventoryId = 4, InventoryQuantity = 2} // pedals
				);
		}

		public DbSet<Inventory>? Inventories { get; set; }
		public DbSet<Product>? Products { get; set; }
		public DbSet<ProductInventory>? ProductInventories { get; set; }
		public DbSet<InventoryTransaction>? InventoryTransactions { get; set; }
		public DbSet<ProductTransaction>? ProductTransactions { get; set; }
	}
}
