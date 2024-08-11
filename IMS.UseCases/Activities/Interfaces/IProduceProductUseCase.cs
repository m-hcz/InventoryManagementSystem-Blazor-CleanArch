using IMS.CoreBusiness;

namespace IMS.UseCases.Activities.Interfaces
{
    public interface IProduceProductUseCase
    {
        Task ExecuteAsync(string productNumber, Product product, int quantity, string doneBy);
    }
}