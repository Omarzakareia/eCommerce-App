using Talabat.Core.Entities.Order_Aggregate;

namespace Talabat.Core.Specifications.Order_Spec;

public class OrderSpecifications : BaseSpecification<Order> 
{
    public OrderSpecifications(string email):base(O=> O.BuyerEmail == email)
    {
        //Eager Loading
        Includes.Add(O => O.DeliveryMethod);
        Includes.Add(O=>O.Items);
        AddOrderByDesc(O => O.OrderDate);
    }
    public OrderSpecifications(string email , int OrderId) : base(O => O.BuyerEmail == email && O.Id == OrderId)
    {
        //Eager Loading
        Includes.Add(O => O.DeliveryMethod);
        Includes.Add(O => O.Items);
        AddOrderByDesc(O => O.OrderDate);
    }

}
