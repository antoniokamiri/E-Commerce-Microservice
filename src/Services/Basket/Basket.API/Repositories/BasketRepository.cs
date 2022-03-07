using Basket.API.Entities;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Basket.API.Repositories
{
    public class BasketRepository : IBasketRepository
    {
        private readonly IDistributedCache _redisCache;

        public BasketRepository(IDistributedCache redisCache)
        {
            _redisCache = redisCache ?? throw new ArgumentNullException(nameof(redisCache));
        }

        public async Task DeleteBasketAsync(string username)
        {
            await _redisCache.RemoveAsync(username);
        }

        public async Task<ShoppingCart> GetBasketAsync(string username, CancellationToken cancellationToken)
        {
            try 
            {
                var basket = await _redisCache.GetStringAsync(username);
                if (String.IsNullOrEmpty(basket)) return null;

                return JsonConvert.DeserializeObject<ShoppingCart>(basket);
            }
            catch (OperationCanceledException ex) // includes TaskCanceledException
            {
                Console.WriteLine("Your submission was canceled." , ex);
                return null;
            }

        }

        public async Task<ShoppingCart> UpdateBasketAsync(ShoppingCart basket, CancellationToken cancellationToken)
        {
            await _redisCache.SetStringAsync(basket.Username, JsonConvert.SerializeObject(basket));

            return await GetBasketAsync(basket.Username, cancellationToken);
        }
    }
}
