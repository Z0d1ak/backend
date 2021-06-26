using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

#nullable disable

namespace gpn
{
    public partial class Operation
    {
        public Operation()
        {
            OperationDeadlines = new HashSet<OperationDeadline>();
        }

        public int Id { get; set; }
        public string EquipmentNumber { get; set; }
        public int? TypeId { get; set; }
        public string Location { get; set; }
        public DateTime? Date { get; set; }
        public string Performer { get; set; }
        public long? PostponedTime { get; set; }

        [ForeignKey("File")]
        public int? FileID { get; set; }
        public FileMetadata? File { get; set; }

        public virtual Equipment EquipmentNumberNavigation { get; set; }
        public virtual OpertationType Type { get; set; }
        public virtual ArrivalOperation ArrivalOperation { get; set; }
        public virtual ICollection<OperationDeadline> OperationDeadlines { get; set; }
    }
}
