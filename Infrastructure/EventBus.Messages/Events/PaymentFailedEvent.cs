using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventBus.Messages.Events
{
    public class PaymentFailedEvent : BaseIntegrationEvent
    {
        public int OrderId { get; set; }
        public string UserName { get; set; }
        public string Reason { get; set; }
        public DateTime TimeStamp { get; set; } = DateTime.UtcNow;
    }
}
