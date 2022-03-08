using Dapper;
using Discount.Grpc.Entities;
using Microsoft.Extensions.Configuration;
using Npgsql;
using System;
using System.Threading.Tasks;

namespace Discount.Grpc.Repositories
{
    public class DiscountRepository : IDiscountRepository
    {
        private readonly IConfiguration _configuration;

        public DiscountRepository(IConfiguration configuration)
        {
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        }

        public async Task<bool> CreateDiscountAsync(Coupon coupon)
        {
            using var connection = new NpgsqlConnection
                    (_configuration.GetValue<string>("DatabaseSettings:ConnectionStrings"));

            connection.Open();

            var affected = await connection.ExecuteAsync
                ("Insert into coupon (Productname, Description, Amount) values (@ProductName, @Description, @Amount)",
                new { Productname = coupon.ProductName, Description = coupon.Description, Amount = coupon.Amount });

            connection.Close();

            if (affected == 0)
                return false;

            return true;
        }

        public async Task<bool> DeleteDiscountAsync(string productname)
        {
            using var connection = new NpgsqlConnection
                    (_configuration.GetValue<string>("DatabaseSettings:ConnectionStrings"));

            connection.Open();

            var affected = await connection.ExecuteAsync
                ("Delete from coupon where Productname = @ProductName",
                new { Productname = productname });

            connection.Close();

            if (affected == 0)
                return false;

            return true;
        }

        public async Task<Coupon> GetDiscountAsync(string productname)
        {
            using var connection = new NpgsqlConnection
                 (_configuration.GetValue<string>("DatabaseSettings:ConnectionStrings"));

            connection.Open();

            var coupon = await connection.QueryFirstOrDefaultAsync<Coupon>
                ("Select * from Coupon where productname = @ProductName", new { ProductName = productname });

            connection.Close();

            if (coupon == null)
                return new Coupon
                { ProductName = "No Discount", Amount = 0, Description = "No Discount Description" };
            return coupon;
        }

        public async Task<bool> UpdateDiscountAsync(Coupon coupon)
        {
            using var connection = new NpgsqlConnection
                   (_configuration.GetValue<string>("DatabaseSettings:ConnectionStrings"));

           connection.Open();

            var affected = await connection.ExecuteAsync
                ("Update coupon Set Productname = @ProductName, Description = @Description, Amount = @Amount where id = @id",
                new { Productname = coupon.ProductName, Description = coupon.Description, Amount = coupon.Amount, id = coupon.Id });

           connection.Close();

            if (affected == 0)
                return false;

            return true;
        }
    }
}
