using System;
using System.Collections.Generic;

namespace Sushi_House.Models
{
    public partial class Sushi
    {
        public Sushi()
        {
            SushiSets = new HashSet<SushiSet>();
        }

        public int SushiId { get; set; }
        public int? SushiTypeId { get; set; }
        public string? SushiName { get; set; }
        public string? SushiPicName { get; set; }
        public string? SushiInqr { get; set; }
        public decimal? SushiPrice { get; set; }

        public virtual Stype? SushiType { get; set; }
        public virtual ICollection<SushiSet> SushiSets { get; set; }
    }
}
