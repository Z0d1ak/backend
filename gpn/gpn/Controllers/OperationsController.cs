using System;
using System.Collections.Generic;
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
    public class OperationsController : ControllerBase
    {
        private readonly GPNContext dataContext;

        public OperationsController(GPNContext dataContext)
        {
            this.dataContext = dataContext;
        }

        [HttpGet]
        public async Task<ActionResult<SearchResponseDto<ReponseOperationDto>>> GetOperations(
           [FromQuery] int? typeID,
           [FromQuery] int? pageNumber,
           [FromQuery] int? companyID)
        {
            var companyIDScoped = this.Request.HttpContext.User.Claims.First(x => x.Type == "CompanyId").Value;
            if (!string.IsNullOrEmpty(companyIDScoped))
            {
                companyID = int.Parse(companyIDScoped);
            }

            if (pageNumber is null || pageNumber < 1)
            {
                pageNumber = 1;
            }

            var operationsQeury = this.dataContext.Operations
                .Include(x => x.File)
                .Include(x => x.Type)
                .Include(x => x.EquipmentNumberNavigation)
                .Where(x =>
                (companyID == null || x.EquipmentNumberNavigation.ComapnyId == companyID)
                || (typeID == null || x.TypeId == typeID)
                )
                .Select(x => new ReponseOperationDto
                {
                    Id = x.Id,
                    Type = x.Type,
                    Location = x.Location,
                    Date = x.Date,
                    File = x.File,
                    Performer = x.Performer,
                    PostponedTime = x.PostponedTime
                });

            var operations = await operationsQeury
                .Skip((pageNumber.Value - 1) * 20)
                .Take(20)
                .ToListAsync();

            var operationsCount = await operationsQeury.CountAsync();

            return this.Ok(new SearchResponseDto<ReponseOperationDto>(operationsCount, operations));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ReponseOperationDto>> GetByID([FromRoute] int id)
        {
            int? companyID = null;
            var companyIDScoped = this.Request.HttpContext.User.Claims.First(x => x.Type == "CompanyId").Value;
            if (!string.IsNullOrEmpty(companyIDScoped))
            {
                companyID = int.Parse(companyIDScoped);
            }

            var operation = await this.dataContext.Operations
                 .Include(x => x.File)
                 .Include(x => x.Type)
                 .Include(x => x.EquipmentNumberNavigation)
                 .Where(x =>
                 id == x.Id
                 && (companyID == null || x.EquipmentNumberNavigation.ComapnyId == companyID)
                 )
                 .Select(x => new ReponseOperationDto
                 {
                     Id = x.Id,
                     Type = x.Type,
                     Location = x.Location,
                     Date = x.Date,
                     File = x.File,
                     Performer = x.Performer,
                     PostponedTime = x.PostponedTime
                 })
                 .FirstOrDefaultAsync();

            if (operation is null)
            {
                return this.NotFound();
            }

            return this.Ok(operation);
        }



    }
}
