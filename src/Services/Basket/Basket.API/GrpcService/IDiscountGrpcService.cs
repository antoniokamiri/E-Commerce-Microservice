using Discount.Grpc.Protos;
using System.Threading.Tasks;

namespace Basket.API.GrpcService
{
    public interface IDiscountGrpcService
    {
        Task<CouponModel> GetDiscountAsync(string productName);
    }
}
