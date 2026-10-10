using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventBus.Messages.Common
{
    public class EventBusConstant
    {
        public const string BasketCheckoutQueue = "basketcheckout-queueB2B";
        public const string OrderCreatedQueue = "ordercreated-queue";
        public const string PaymentCompletedQueue = "paymentcompleted-queue";
        public const string PaymentFailedQueue = "paymentfailed-queue";
    }
}
