using System;
using System.Collections.Generic;

#nullable disable

namespace gpn
{
    public partial class User
    {
        public int Id { get; set; }
        public int? ComapnyId { get; set; }
        public string Role { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }

        public virtual Company Comapny { get; set; }
    }
}
