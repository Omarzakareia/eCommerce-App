namespace Talabat.Core.Entities.Order_Aggregate;
public class Order : BaseEntity
{
    public Order()
    {
        
    }
    public Order(string buyerEmail, Address shippingAddress, DeliveryMethod deliveryMethod, ICollection<OrderItem> items, decimal subTotal, string paymentIntentId)
    {
        BuyerEmail = buyerEmail;
        ShippingAddress = shippingAddress;
        DeliveryMethod = deliveryMethod;
        Items = items;
        SubTotal = subTotal;
        PaymentIntentId = paymentIntentId;
    }

    public string BuyerEmail { get; set; }
    public DateTimeOffset OrderDate { get; set; } = DateTimeOffset.Now; // Date Of Order Creation [Now]
    public OrderStatus Status { get; set; } // Status Of Order At First [Pending]
    public Address ShippingAddress { get; set; }
    //public int DeliveryMethodId { get; set; } // FK
    public DeliveryMethod DeliveryMethod { get; set; } // Navigation Property 
    public ICollection<OrderItem> Items { get; set; } = new HashSet<OrderItem>(); // Navigation Property [Many]
    public decimal SubTotal { get; set; }       // SubTotal = Price Of Product * Quantity
    //[NotMapped]
    //public decimal Total { get => SubTotal + DeliveryMethod.Cost; }          // Total =  SubTotal + DeliveryMethod Cost
    public decimal GetTotal() =>  SubTotal + DeliveryMethod.Cost;

    public string PaymentIntentId { get; set; } 


}
