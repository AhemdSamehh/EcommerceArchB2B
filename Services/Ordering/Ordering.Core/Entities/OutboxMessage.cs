using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ordering.Core.Entities
{
    public class OutboxMessage : EntityBase
    {
        public string Type { get; set; }
        public string Content { get; set; }
        public string CorrelationId { get; set; }
        public DateTime OccurredOn { get; set; }
        public DateTime? ProcessedOn { get; set; }
        public bool IsProcessed => ProcessedOn.HasValue;
        public string? Error { get; set; }
    }
}
