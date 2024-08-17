using IMS.WebApp.Components.Account;
using IMS.Plugins.EFCoreSqlServer;
using IMS.Plugins.InMemory;
using IMS.UseCases.Activities;
using IMS.UseCases.Activities.Interfaces;
using IMS.UseCases.Inventories;
using IMS.UseCases.Inventories.Interfaces;
using IMS.UseCases.PluginInterfaces;
using IMS.UseCases.Products;
using IMS.UseCases.Products.Interfaces;
using IMS.UseCases.Reports;
using IMS.UseCases.Reports.Interfaces;
using IMS.WebApp.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Server;
using Microsoft.AspNetCore.Hosting.StaticWebAssets;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using IMS.WebApp.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
	.AddInteractiveServerComponents();

var dbString = builder.Configuration.GetConnectionString("IMS");
var dbAccountsString = builder.Configuration.GetConnectionString("IMSAccounts");

builder.Services.AddDbContextFactory<IMSContext>(options =>
{
	options.UseMySql(dbString, ServerVersion.AutoDetect(dbString), opt =>
	{
		opt.EnableStringComparisonTranslations();
	});
});


// identity
builder.Services.AddCascadingAuthenticationState();
builder.Services.AddScoped<IdentityUserAccessor>();
builder.Services.AddScoped<IdentityRedirectManager>();
builder.Services.AddScoped<AuthenticationStateProvider, ServerAuthenticationStateProvider>();

builder.Services.AddAuthorization(options =>
{
	options.AddPolicy("Admin", policy => policy.RequireClaim("Department", "Administration"));
	options.AddPolicy("Inventory", policy => policy.RequireClaim("Department", "InventoryManagement"));
	options.AddPolicy("Sales", policy => policy.RequireClaim("Department", "Sales"));
	options.AddPolicy("Purchasers", policy => policy.RequireClaim("Department", "Purchasing"));
	options.AddPolicy("Productions", policy => policy.RequireClaim("Department", "ProductionManager"));
});
builder.Services.AddAuthentication(options =>
{
	options.DefaultScheme = IdentityConstants.ApplicationScheme;
	options.DefaultSignInScheme = IdentityConstants.ExternalScheme;
})
	.AddIdentityCookies();

//builder.Services.AddDbContext<ApplicationDbContext>(options =>
//	options.UseSqlServer(connectionString));

builder.Services.AddDbContextFactory<ApplicationDbContext>(options =>
{
	options.UseMySql(dbAccountsString, ServerVersion.AutoDetect(dbAccountsString), opt =>
	{
		opt.EnableStringComparisonTranslations();
	});
});

builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddIdentityCore<ApplicationUser>(options => options.SignIn.RequireConfirmedAccount = true)
	.AddEntityFrameworkStores<ApplicationDbContext>()
	.AddSignInManager()
	.AddDefaultTokenProviders();

builder.Services.AddSingleton<IEmailSender<ApplicationUser>, IdentityNoOpEmailSender>();


// Singleton - created only once
// Transient - created every time when is required
// Scoped - stores DI as long as SignalR connection lives, Is the same as Singleton when used in WebAssembly


if (builder.Environment.IsEnvironment("Testing"))
{
	// pokud nefungují css
	StaticWebAssetsLoader.UseStaticWebAssets(builder.Environment, builder.Configuration);

	// inventory
	builder.Services.AddSingleton<IInventoryRepository, InventoryRepository>();
	// product
	builder.Services.AddSingleton<IProductRepository, ProductRepository>();
	// purchase
	builder.Services.AddSingleton<IInventoryTransactionRepository, InventoryTransactionRepository>();
	// produce
	builder.Services.AddSingleton<IProductTransactionRepository, ProductTransactionRepository>();
}
else
{
	// ef core není thread save -> transcient
	// inventory
	builder.Services.AddTransient<IInventoryRepository, InventoryEFCoreRepository>();
	// product
	builder.Services.AddTransient<IProductRepository, ProductEFCoreRepository>();
	// purchase
	builder.Services.AddTransient<IInventoryTransactionRepository, InventoryTransactionEFCoreRepository>();
	// produce
	builder.Services.AddTransient<IProductTransactionRepository, ProductTransactionEFCoreRepository>();
}

// inventory
builder.Services.AddTransient<IViewInventoriesByNameUseCase, ViewInventoriesByNameUseCase>();
builder.Services.AddTransient<IViewInventoryByIdUseCase, ViewInventoryByIdUseCase>();
builder.Services.AddTransient<IAddInventoryUseCase, AddInventoryUseCase>();
builder.Services.AddTransient<IEditInventoryUseCase, EditInventoryUseCase>();
builder.Services.AddTransient<IDeleteInventoryUseCase, DeleteInventoryUseCase>();

// product
builder.Services.AddTransient<IViewProductsByNameUseCase, ViewProductsByNameUseCase>();
builder.Services.AddTransient<IViewProductByIdUseCase, ViewProductByIdUseCase>();
builder.Services.AddTransient<IAddProductUseCase, AddProductUseCase>();
builder.Services.AddTransient<IEditProductUseCase, EditProductUseCase>();
builder.Services.AddTransient<IDeleteProductUseCase, DeleteProductUseCase>();

// activity
// purchase
builder.Services.AddTransient<IPurchaseInventoryUseCase, PurchaseInventoryUseCase>();

// produce
builder.Services.AddTransient<IProduceProductUseCase, ProduceProductUseCase>();

// sell
builder.Services.AddTransient<ISellProductUseCase, SellProductUseCase>();

// reports
builder.Services.AddTransient<ISearchInventoryTransactionsUseCase, SearchInventoryTransactionsUseCase>();
builder.Services.AddTransient<ISearchProductTransactionsUseCase, SearchProductTransactionsUseCase>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
	app.UseMigrationsEndPoint();
}
else
{
	app.UseExceptionHandler("/Error", createScopeForErrors: true);
	// The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
	app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>()
	.AddInteractiveServerRenderMode();

// Add additional endpoints required by the Identity /Account Razor components.
app.MapAdditionalIdentityEndpoints();

app.Run();
