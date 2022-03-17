using Microsoft.EntityFrameworkCore;
using Ordering.Application.Contracts.Persistence;
using Ordering.Domain.Entities;
using Ordering.Infrastructure.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ordering.Infrastructure.Repositories
{
    public class OrderRepository : RepositoriesBase<Order>, IOrderRepository
    {
        // OOP principles thats why we need the consructure
        public OrderRepository(OrderContext orderContext) : base(orderContext)
        {
        }

        public async Task<IEnumerable<Order>> GetOrderByUserNameAsync(string userName)
        {
            var orderList = await _orderContext.Orders.Where(o => o.UserName == userName).ToListAsync();
            return orderList;
        }
    }
}
