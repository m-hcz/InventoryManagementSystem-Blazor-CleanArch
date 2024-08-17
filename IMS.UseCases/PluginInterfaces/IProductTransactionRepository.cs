using IMS.CoreBusiness;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMS.UseCases.PluginInterfaces
{
	public interface IProductTransactionRepository
	{
		Task<IEnumerable<ProductTransaction>> GetProductTransactionsAsync(string productName, DateTime? dateFrom, DateTime? toDate, ProductTransactionType? transactionType);
		Task ProduceAsync(string productNumber, Product product, int quantity, string doneBy);
		Task SellProductAsync(string salesOrderNumber, Product product, int quantity, double unitPrice, string doneBy);
	}
}
