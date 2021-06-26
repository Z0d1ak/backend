using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace gpn.Dto
{
    public class LoginResponseDto
    {
        public string Token { get; set; }

        public UserDto User { get; set; }

        public CompanyDto Company {get; set;}
    }
}
