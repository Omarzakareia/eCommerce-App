using Microsoft.AspNetCore.Mvc;
using Talabat.APIs.Errors;
using Talabat.APIs.Helpers;
using Talabat.Core;
using Talabat.Core.Repositories;
using Talabat.Core.Services;
using Talabat.Repository;
using Talabat.Service;

namespace Talabat.APIs.Extensions;

public static class ApplicationServicesExtension
{
    public static IServiceCollection AddApplicationService(this IServiceCollection Services)
    {
        Services.AddSingleton<IResponseCacheService, ResponseCacheService>();
        Services.AddScoped(typeof(IUnitOfWork), typeof(UnitOfWork));
        Services.AddScoped(typeof(IOrderService), typeof(OrderService));
        Services.AddScoped(typeof(IPaymentService), typeof(PaymentService));
        Services.AddScoped(typeof(IBasketRepository), typeof(BasketRepository));
        //builder.Services.AddAutoMapper(M=>M.AddProfile(new MappingProfiles()));
        Services.AddAutoMapper(typeof(MappingProfiles));
        Services.Configure<ApiBehaviorOptions>(Options =>
        {
            Options.InvalidModelStateResponseFactory = (actionContext) =>
            {
                // ModelState => Dic[KeyValuePair]
                //Key => Name of parameter
                // Value => Error
                var errors = actionContext.ModelState.Where(p => p.Value.Errors.Count() > 0)
                                                                                 .SelectMany(P => P.Value.Errors)
                                                                                 .Select(E => E.ErrorMessage)
                                                                                 .ToArray();
                var ValidtionErrorResponse = new ApiValidationErrorResponse()
                {
                    Errors = errors
                };
                return new BadRequestObjectResult(ValidtionErrorResponse);
            };
        });
        return Services;
    }
}
