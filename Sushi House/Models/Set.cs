using System;
using System.Collections.Generic;

namespace Sushi_House.Models
{
    public partial class Set
    {
        public Set()
        {
            SushiSets = new HashSet<SushiSet>();
        }

        public int SetId { get; set; }
        public string? SetName { get; set; }
        public string? SetPicName { get; set; }

        public virtual ICollection<SushiSet> SushiSets { get; set; }
    }
}
