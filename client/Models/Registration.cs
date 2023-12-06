using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace client.Models
{
    public class Registration
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Weight { get; set; }
        List<Passanger> Passangers { get; set; } = new();
    }
}
