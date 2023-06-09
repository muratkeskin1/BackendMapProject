using OfficeOpenXml;
using StajProje.Helper;
using StajProje.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace StajProje.Service
{
    public class ExcelReportService
    {
        private readonly ApplicationDbContext _context;
        public ExcelReportService(ApplicationDbContext context)
        {
            _context = context;
        }
        public void TestExcel()
        {
            using (ExcelPackage excelPackage = new ExcelPackage())
            {
                ExcelWorksheet worksheet = excelPackage.Workbook.Worksheets.Add("Atm Table");

                List<ATM> atmlist = _context.ATMs.ToList();
                worksheet.Cells["A2"].LoadFromCollection(atmlist);
                //kolonlar
                worksheet.Cells["A1"].Value = "id";
                worksheet.Cells["B1"].Value = "atmId";
                worksheet.Cells["C1"].Value = "Lat.";
                worksheet.Cells["D1"].Value = "Long.";
                worksheet.Cells["E1"].Value = "Py";
                worksheet.Cells["F1"].Value = "Pç";
                worksheet.Cells["G1"].Value = "TL";
                worksheet.Cells["H1"].Value = "Usd";
                worksheet.Cells["I1"].Value = "euro";
                worksheet.Cells["J1"].Value = "status";
                FileInfo fi = new FileInfo("test.xlsx");
                excelPackage.SaveAs(fi);
            }

        }
    }
}
