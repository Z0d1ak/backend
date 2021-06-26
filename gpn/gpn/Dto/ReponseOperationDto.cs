using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace gpn.Dto
{
    public class ReponseOperationDto
    {
        public int Id { get; set; }
        public string EquipmentNumber { get; set; }
        public OpertationType Type { get; set; }
        public string Location { get; set; }
        public DateTime? Date { get; set; }
        public string Performer { get; set; }
        public long? PostponedTime { get; set; }
        public FileMetadata? File { get; set; }

    }
}
