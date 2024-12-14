using StackExchange.Redis;
using System.Text.Json;
using Talabat.Core.Entities;
using Talabat.Core.Repositories;

namespace Talabat.Repository;

public class BasketRepository : IBasketRepository
{
    private readonly IDatabase _database;

    // ASK CLR for object from class that implements interfaceIConnectionMultiplexer 
    public BasketRepository(IConnectionMultiplexer redis)
    {
        _database = redis.GetDatabase();
    }
    public async Task<bool> DeleteBasketAsync(string BasketId)
    {
        return await _database.KeyDeleteAsync(BasketId);
    }

    public async Task<CustomerBasket?> GetBasketAsync(string BasketId)
    {
        var Basket = await _database.StringGetAsync(BasketId);
        //if (Basket.IsNull) return null;
        //else 
        //    var ReturnedBasket = JsonSerializer.Deserialize<CustomerBasket>(Basket);

        return Basket.IsNull ? null : JsonSerializer.Deserialize<CustomerBasket>(Basket);

    }

    public async Task<CustomerBasket?> UpdateBasketAsync(CustomerBasket Basket)
    {
        var JsonBasket = JsonSerializer.Serialize(Basket);
        var CreatedOrUpdated = await _database.StringSetAsync(Basket.Id, JsonBasket, TimeSpan.FromDays(1));
        if (!CreatedOrUpdated) return null;

        return await GetBasketAsync(Basket.Id);
    }
}
