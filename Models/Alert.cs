using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Malshinon.Models
{
    public class Alert
    {
        public int Id { get; set; }
        public int TargetId { get; set; }
        public DateTime CreatedAt { get; set; }
        public string Reason { get; set; }
    }
}
