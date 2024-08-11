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
		Task ProduceAsync(string productNumber, Product product, int quantity, string doneBy);
	}
}
