using Talabat.Core.Entities.Order_Aggregate;
namespace Talabat.Core.Services;
public interface IOrderService
{
    Task<Order?> CreateOrderAsync(string BuyerEmail, string BasketId, int DeliveryMethodId, Address ShippingAddress);
    Task<IReadOnlyList<Order>> GetOrdersForSpecificationAsync(string BuyerEmail);
    Task<Order> GetOrderByIdForSpecificUserAsync(string BuyerEmail, int OrderId);
    Task<IReadOnlyList<DeliveryMethod>> GetDeliveryMethodsAsync();
}
