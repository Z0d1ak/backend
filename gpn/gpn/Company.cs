using System;
using System.Collections.Generic;

#nullable disable

namespace gpn
{
    public partial class Company
    {
        public Company()
        {
            Equipment = new HashSet<Equipment>();
            Users = new HashSet<User>();
        }

        public int Id { get; set; }
        public string Name { get; set; }

        public virtual ICollection<Equipment> Equipment { get; set; }
        public virtual ICollection<User> Users { get; set; }
    }
}
