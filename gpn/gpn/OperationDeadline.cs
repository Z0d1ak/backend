using System;
using System.Collections.Generic;

#nullable disable

namespace gpn
{
    public partial class OperationDeadline
    {
        public string EquipmentNumber { get; set; }
        public int? OperationId { get; set; }
        public int TypeId { get; set; }

        public virtual Equipment EquipmentNumberNavigation { get; set; }
        public virtual Operation Operation { get; set; }
        public virtual OpertationType Type { get; set; }
    }
}
