using System;
using System.Collections.Generic;

namespace Sushi_House.Models
{
    public partial class Stype
    {
        public Stype()
        {
            Sushis = new HashSet<Sushi>();
        }

        public int StypeId { get; set; }
        public string? StypeName { get; set; }

        public virtual ICollection<Sushi> Sushis { get; set; }
    }
}
