using System;
using System.Collections.Generic;

namespace Sushi_House.Models
{
    public partial class SushiSet
    {
        public int SushiSetId { get; set; }
        public int? SushiSetSetId { get; set; }
        public int? SushiSetSushiId { get; set; }

        public virtual Set? SushiSetSet { get; set; }
        public virtual Sushi? SushiSetSushi { get; set; }
    }
}
