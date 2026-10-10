using System;
using System.Collections.Generic;
using System.Text;

namespace Ordering.Tool.Repositories
{
    public interface IOrderEventPublisher
    {
        Task PublishOrderCreatedAsync(
            int orderId,
            decimal totalPrice,
            CancellationToken cancellationToken);
    }
}
