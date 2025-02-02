﻿using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

using ClosedXML.Excel;

using DocumentFormat.OpenXml.Bibliography;

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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
                worksheet.Cell($"A{i}").Value = "Номер оборудования";
                worksheet.Cell($"B{i}").Value = "Компания";


                worksheet.Cell($"C{i}").Value = "Тип оборудовани";

                worksheet.Cell($"D{i}").Value = "Тип операции";
                worksheet.Cell($"E{i}").Value = "Ответственный";
                worksheet.Cell($"F{i}").Value = "Локация";
                worksheet.Cell($"G{i}").Value = "Время просрочки";
                i++;
                foreach (var el in dt)
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
// var cd = new System.Net.Mime.ContentDisposition
// {
//     FileName = "report.xlsx",
//
//     // always prompt the user for downloading, set to true if you want 
//     // the browser to try to show the file inline
//     Inline = false
// };
            return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "report.xlsx");
        }
    }
}
