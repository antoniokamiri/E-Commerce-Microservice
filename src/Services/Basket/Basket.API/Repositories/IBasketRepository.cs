using Basket.API.Entities;
using System.Threading;
using System.Threading.Tasks;

namespace Basket.API.Repositories
{
    public interface IBasketRepository
    {
        Task<ShoppingCart> GetBasketAsync(string username, CancellationToken cancellationToken);
        Task<ShoppingCart> UpdateBasketAsync(ShoppingCart basket, CancellationToken cancellationToken);
        Task DeleteBasketAsync (string username);
    }
}
