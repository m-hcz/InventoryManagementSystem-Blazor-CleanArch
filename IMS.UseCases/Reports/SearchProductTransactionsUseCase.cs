using IMS.CoreBusiness;
using IMS.UseCases.PluginInterfaces;
using IMS.UseCases.Reports.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMS.UseCases.Reports
{
	public class SearchProductTransactionsUseCase(IProductTransactionRepository _productTransactionRepository) : ISearchProductTransactionsUseCase
	{
		public async Task<IEnumerable<ProductTransaction>> ExecuteAsync(string productName, DateTime? dateFrom, DateTime? toDate, ProductTransactionType? transactionType)
		{
			if (toDate.HasValue) toDate = toDate.Value.AddDays(1);

			return await _productTransactionRepository.GetProductTransactionsAsync(productName, dateFrom, toDate, transactionType);
		}
	}
}
