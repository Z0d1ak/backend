using System;
using System.Collections.Generic;

#nullable disable

namespace gpn
{
    public partial class ArrivalOperation
    {
        public int Id { get; set; }
        public string LogisticCompany { get; set; }

        public virtual Operation IdNavigation { get; set; }
    }
}
