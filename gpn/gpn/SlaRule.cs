using System;
using System.Collections.Generic;

#nullable disable

namespace gpn
{
    public partial class SlaRule
    {
        public int Id { get; set; }
        public int? TypeId { get; set; }
        public int? NextTypeId { get; set; }
        public string OptionType { get; set; }
        public long? Duration { get; set; }

        public virtual OpertationType NextType { get; set; }
        public virtual OpertationType Type { get; set; }
    }
}
