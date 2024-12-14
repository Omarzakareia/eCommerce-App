
using Talabat.Core.Entities.Order_Aggregate;

namespace Talabat.Core.Specifications.Order_Spec;

public class OrderWithPaymentIntentSpec : BaseSpecification<Order>
{
    public OrderWithPaymentIntentSpec(string PaymentIntentId):base(Order => Order.PaymentIntentId == PaymentIntentId)
    {
        
    } 
}
