using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using gpn.Dto;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

using ClosedXML;
using ClosedXML.Excel;
using System.IO;
using System.Net.Http;
using System.Net;
using System.Net.Http.Headers;

namespace gpn.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize]
    public class ReportController : ControllerBase 
    {
        private readonly GPNContext dataContetext;

        public ReportController(GPNContext dataContetext)
        {
            this.dataContetext = dataContetext;
        }

        [HttpGet]
        public async Task<IActionResult> GetReport(DateTime startDate, DateTime endDate, int? companyID)
        {
            var companyIDScoped = this.Request.HttpContext.User.Claims.FirstOrDefault(x => x.Type == "CompanyId")?.Value;
            if (!string.IsNullOrEmpty(companyIDScoped))
            {
                companyID = int.Parse(companyIDScoped);
            }

            var stream = new MemoryStream();

            var dt = await this.dataContetext.Operations
                .Include(x => x.Type)
                .Include(x => x.EquipmentNumberNavigation).ThenInclude(x => x.Comapny)
                .Where(x =>
                  x.Date >= startDate
                  //&& x.PostponedTime > 0
                  && x.Date <= endDate
                  && (companyID == null || x.EquipmentNumberNavigation.ComapnyId == companyID))
            .ToListAsync();

            using (var workbook = new XLWorkbook())
            {
                var worksheet = workbook.Worksheets.Add("Отчет");

                var i = 1;
                foreach(var el in dt)
                {
                    worksheet.Cell($"A{i}").Value = el.EquipmentNumberNavigation?.Number;
                    worksheet.Cell($"B{i}").Value = el.EquipmentNumberNavigation?.Comapny.Name;


                    worksheet.Cell($"C{i}").Value = el.EquipmentNumberNavigation?.Type;

                    worksheet.Cell($"D{i}").Value = el.Type.Name;
                    worksheet.Cell($"E{i}").Value = el.Performer;
                    worksheet.Cell($"F{i}").Value = el.Location;
                    if (el.PostponedTime.HasValue)
                    {
                        worksheet.Cell($"G{i}").Value = new TimeSpan(el.PostponedTime.Value).ToString(@"hh\:mm\:ss");
                    }
                    i++;
                }
                workbook.SaveAs(stream);
            }

            return File(stream, "application/octet-stream", "report.xlsx");
        }
    }
}
