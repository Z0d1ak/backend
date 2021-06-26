using System;
using System.Collections.Generic;

#nullable disable

namespace gpn
{
    public partial class OpertationType
    {
        public OpertationType()
        {
            OperationDeadlines = new HashSet<OperationDeadline>();
            Operations = new HashSet<Operation>();
            SlaRuleNextTypes = new HashSet<SlaRule>();
            SlaRuleTypes = new HashSet<SlaRule>();
        }

        public int Id { get; set; }
        public string Name { get; set; }

        public virtual ICollection<OperationDeadline> OperationDeadlines { get; set; }
        public virtual ICollection<Operation> Operations { get; set; }
        public virtual ICollection<SlaRule> SlaRuleNextTypes { get; set; }
        public virtual ICollection<SlaRule> SlaRuleTypes { get; set; }
    }
}
