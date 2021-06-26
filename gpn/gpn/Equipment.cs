using System;
using System.Collections.Generic;

#nullable disable

namespace gpn
{
    public partial class Equipment
    {
        public Equipment()
        {
            InverseParent = new HashSet<Equipment>();
            OperationDeadlines = new HashSet<OperationDeadline>();
            Operations = new HashSet<Operation>();
        }

        public string Number { get; set; }
        public string Type { get; set; }
        public int? ComapnyId { get; set; }
        public string Name { get; set; }
        public string Location { get; set; }
        public string State { get; set; }
        public string ParentId { get; set; }

        public virtual Company Comapny { get; set; }
        public virtual Equipment Parent { get; set; }
        public virtual ICollection<Equipment> InverseParent { get; set; }
        public virtual ICollection<OperationDeadline> OperationDeadlines { get; set; }
        public virtual ICollection<Operation> Operations { get; set; }
    }
}
