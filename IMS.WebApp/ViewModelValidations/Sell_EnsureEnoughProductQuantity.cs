using IMS.WebApp.ViewModels;
using System.ComponentModel.DataAnnotations;

namespace IMS.WebApp.ViewModelValidations
{
	public class Sell_EnsureEnoughProductQuantity : ValidationAttribute
	{
		protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
		{
			var sellViewModel = validationContext.ObjectInstance as SellViewModel;

			if (sellViewModel != null)
			{
				if (sellViewModel.Product != null)
				{
						if (sellViewModel.Product.Quantity < sellViewModel.QuantityToSell)
							return new ValidationResult($"There is not enouth product ({sellViewModel.Product.Name}). There is only {sellViewModel.Product.Quantity} in the warehouse.", new[] { validationContext.MemberName });
				}
			}

			return ValidationResult.Success;
		}
	}
}
