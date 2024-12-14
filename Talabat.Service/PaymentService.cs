using Microsoft.Extensions.Configuration;
using Stripe;
using Talabat.Core;
using Talabat.Core.Entities;
using Talabat.Core.Entities.Order_Aggregate;
using Talabat.Core.Repositories;
using Talabat.Core.Services;
using Talabat.Core.Specifications.Order_Spec;
using Product = Talabat.Core.Entities.Product;

namespace Talabat.Service;

public class PaymentService : IPaymentService
{
    private readonly IConfiguration _configuration;
    private readonly IBasketRepository _basketRepository;
    private readonly IUnitOfWork _unitOfWork;

    public PaymentService(IConfiguration configuration,
        IBasketRepository basketRepository
        , IUnitOfWork unitOfWork)
    {
        this._configuration = configuration;
        this._basketRepository = basketRepository;
        this._unitOfWork = unitOfWork;
    }
    public async Task<CustomerBasket?> CreateOrUpdatePaymentIntent(string BasketId)
    {
        // Secret Key
        StripeConfiguration.ApiKey = _configuration["StripeKeys:Secretkey"];

        // Get basket 
        var Basket = await _basketRepository.GetBasketAsync(BasketId);
        if (Basket == null) return null;
        var ShippingPrice = 0M; // Decimal
        if (Basket.DeliveryMethodId.HasValue)
        {
            var DeliveryMethod = await _unitOfWork.Repository<DeliveryMethod>().GetByIdAsync(Basket.DeliveryMethodId.Value);
            ShippingPrice = DeliveryMethod.Cost;
        }

        // Total = Subtotal + 0M.Cost
        if (Basket.Items.Count > 0)
        {
            foreach (var item in Basket.Items)
            {
                var Product = await _unitOfWork.Repository<Product>().GetByIdAsync(item.Id);
                if (item.Price != Product.Price)
                    item.Price = Product.Price;
            }
        }
        var SubTotal = Basket.Items.Sum(item => item.Price * item.Quantity);

        //Create Payment Intent
        var Service = new PaymentIntentService();
        PaymentIntent paymentIntent;
        if (string.IsNullOrEmpty(Basket.PaymentIntentId)) // Create
        {
            var Options = new PaymentIntentCreateOptions()
            {
                Amount = (long)(SubTotal * 100 + ShippingPrice * 100),
                Currency = "usd",
                PaymentMethodTypes = new List<string> { "card" }
            };
            paymentIntent = await Service.CreateAsync(Options);
            Basket.PaymentIntentId = paymentIntent.Id;
            Basket.ClientSecret = paymentIntent.ClientSecret;
        }
        else //Update 
        {
            var Options = new PaymentIntentUpdateOptions()
            {
                Amount = (long)(SubTotal * 100 + ShippingPrice * 100)
            };
            paymentIntent = await Service.UpdateAsync(Basket.PaymentIntentId, Options);
            Basket.PaymentIntentId = paymentIntent.Id;
            Basket.ClientSecret = paymentIntent.ClientSecret;
            
        }

        await _basketRepository.UpdateBasketAsync(Basket);
        return Basket;
    }

    public async Task<Order> UpdatePaymentIntentToSucceedOrFailed(string PaymentIntentId, bool flag)
    {
        var Spec = new OrderWithPaymentIntentSpec(PaymentIntentId);
        var Order = await _unitOfWork.Repository<Order>().GetEntityWithSpecAsync(Spec);
        if (flag)
        {
            Order.Status = OrderStatus.PaymentReceived;
        } 
        else 
        {
            Order.Status = OrderStatus.PaymentFailed;
        }
        _unitOfWork.Repository<Order>().Update(Order);
        await _unitOfWork.CompleteAsync();
        return Order;
    }
}
