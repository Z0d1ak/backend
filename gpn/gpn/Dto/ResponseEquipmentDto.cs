using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace gpn.Dto
{
    public class ResponseEquipmentDto
    {
        public string Number { get; set; }
        public string Type { get; set; }

        public CompanyDto Company { get; set; }
        public string Name { get; set; }
        public string Location { get; set; }
        public string State { get; set; }
    }
}
