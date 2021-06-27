using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Threading.Tasks;

using gpn.Dto;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace gpn.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class EquipmentController : ControllerBase
    {
        private readonly GPNContext dataContext;

        public EquipmentController(GPNContext dataContext)
        {
            this.dataContext = dataContext;
        }

        [HttpGet]
        public async Task<ActionResult<SearchResponseDto<ResponseEquipmentDto>>> GetEquipments(
            [FromQuery] string? number,
            [FromQuery] int? pageNumber,
            [FromQuery] int? companyID,
            [FromQuery] string? parentNumber)
        {
            var companyIDScoped = this.Request.HttpContext.User.Claims.First(x => x.Type == "CompanyId").Value;
            if (!string.IsNullOrEmpty(companyIDScoped))
            {
                companyID = int.Parse(companyIDScoped);
            }

            if(pageNumber is null || pageNumber < 1)
            {
                pageNumber = 1;
            }
            if(number is not null)
            {
                number += '%';
            }

            var eqipmentsQeury = this.dataContext.Equipment
                .Include(x => x.Comapny)
                .Where(x =>
                (string.IsNullOrEmpty(parentNumber) || x.ParentId.Equals(parentNumber, StringComparison.OrdinalIgnoreCase))
                && (string.IsNullOrEmpty(number) || EF.Functions.Like(x.Number, number))
                && (companyID == null || x.ComapnyId == companyID)
                )
                .Select(x => new ResponseEquipmentDto
                {
                    Number = x.Number,
                    Type = x.Type,
                    Location = x.Location,
                    State = x.State,
                    Name = x.Name,
                    Company = new CompanyDto
                    {
                        ID = x.Comapny.Id,
                        Name = x.Comapny.Name,
                    }
                });

            var eqipments = await eqipmentsQeury
                .Skip((pageNumber.Value - 1) * 10)
                .Take(10)
                .ToListAsync();

            var eqipmentsCount = await eqipmentsQeury.CountAsync();

            return this.Ok(new SearchResponseDto<ResponseEquipmentDto>(eqipmentsCount, eqipments));
        }

        [HttpGet("{number}")]
        public async Task<ActionResult<ResponseEquipmentDto>> GetByID([FromRoute]string number)
        {
            int? companyID = null;
            var companyIDScoped = this.Request.HttpContext.User.Claims.First(x => x.Type == "CompanyId").Value;
            if (!string.IsNullOrEmpty(companyIDScoped))
            {
                companyID = int.Parse(companyIDScoped);
            }

            var equipment = await this.dataContext.Equipment.Include(x => x.Comapny)
                .Where(x =>
                x.Number.ToLower() == number.ToLower()
                && (companyID == null || x.ComapnyId == companyID)
                )
                .Select(x => new ResponseEquipmentDto
                {
                    Number = x.Number,
                    Type = x.Type,
                    Location = x.Location,
                    State = x.State,
                    Name = x.Name,
                    Company = new CompanyDto
                    {
                        ID = x.Comapny.Id,
                        Name = x.Comapny.Name,
                    }
                })
                .FirstOrDefaultAsync();

            if(equipment is null)
            {
                return this.NotFound();
            }

            return this.Ok(equipment);
        }
    }
}
