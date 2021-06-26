using System;
using System.Collections.Generic;
using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using gpn.Dto;

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace gpn.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly GPNContext dataContext;

        public AuthController(GPNContext dataContext)
        {
            this.dataContext = dataContext;
        }

        /// <summary>
        /// Авторизация
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// 
        ///     {
        ///         "email": "admin@admin.com",
        ///         "password": "admin"
        ///     }
        /// </remarks>
        [HttpPost("/login")]
        public async Task<ActionResult<LoginResponseDto>> Auth([FromBody]LoginRequestDto requestDto)
        {
            var email = requestDto.Email.ToLower();
            var resp = await this.dataContext.Users
                .Include(x => x.Comapny)
                .Where(x => x.Email == email && x.Password == requestDto.Password)
                .Select(x => new LoginResponseDto
                {
                    User = new UserDto
                    {
                        Id = x.Id,
                        Email = x.Email,
                        Role = x.Role
                    },
                    Company = x.Comapny == null ? null : new CompanyDto
                    {
                        ID = x.Comapny.Id,
                        Name = x.Comapny.Name
                    }
                })
                .FirstOrDefaultAsync();

            if(resp is null)
            {
                return this.Unauthorized("Email адрес или пароль не верные");
            }

            var claims = new List<Claim>()
            {
                new Claim("UserId", resp.User.Id.ToString()),
                new Claim(ClaimTypes.Role, resp.User.Role.ToString())
            };

            if(resp.Company is not null)
            {
                claims.Add(new Claim("CompanyId", resp.Company.ID.ToString()));
            }

            var key = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(
                Consts.SecurityKey));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddDays(30),
                SigningCredentials = creds
            };

            var tokenHandler = new JwtSecurityTokenHandler();

            var token = tokenHandler.CreateToken(tokenDescriptor);

            resp.Token = tokenHandler.WriteToken(token);

            return this.Ok(resp);
        }
    }
}
