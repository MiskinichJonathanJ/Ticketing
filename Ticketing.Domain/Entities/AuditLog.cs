using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ticketing.Domain.Entities
{
    public class AuditLog
    {
        public Guid Id { get; set; }
        public int? UserId { get; set; } 
        public string Action { get; set; }
        public string EntityName { get; set; }
        public string EntityId { get; set; }
        public string Details { get; set; }
        public DateTime CreatedAt { get; set; }

        public virtual User? User { get; set; }
    }
}
