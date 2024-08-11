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
	}
}
