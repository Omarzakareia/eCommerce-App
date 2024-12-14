using Talabat.Core.Entities;
using Talabat.Core.Entities.Order_Aggregate;

namespace Talabat.Core.Services;
public interface IPaymentService
{
    // Function To Create Or Update Payment Intent
    Task<CustomerBasket?> CreateOrUpdatePaymentIntent(string BasketId);
    Task<Order> UpdatePaymentIntentToSucceedOrFailed(string PaymentIntentId, bool flag);
}
