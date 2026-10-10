using Microsoft.EntityFrameworkCore;
using Ordering.Core.Entities;
using Ordering.Core.Repositories;
using Ordering.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ordering.Infrastructure.Repositories
{
    public class OrderRepository : RepositoryBase<Order>, IOrderRepository
    {
        public OrderRepository(OrderContext orderContext) : base(orderContext)
        {
        }

        public async Task AddOutboxMessageAsync(OutboxMessage outboxMessage)
        {
            await  _orderContext.outboxMessages.AddAsync(outboxMessage);
            await _orderContext.SaveChangesAsync();
        }

        public async Task<IEnumerable<Order>> GetOrdersByUserName(string userName)
        {
            var orderList = await _orderContext.Orders.AsNoTracking().Where(o => o.UserName == userName).ToListAsync();
            return orderList;
        }
    }
}
