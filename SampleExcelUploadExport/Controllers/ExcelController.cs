using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using SampleExcelUploadExport.Helpers;
using OfficeOpenXml;

namespace SampleExcelUploadExport.Controllers
{
    public class ExcelController : Controller
    {
        // GET: Excel
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// Upload Excel sheet data
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult UploadExcel(HttpPostedFileBase file)
        {
            try
            {
                if (file != null && file.ContentLength > 0)
                {
                    // Create a temporary file path
                    var tempFilePath = Path.GetTempFileName();

                    // Save the uploaded file to the temporary path
                    file.SaveAs(tempFilePath);

                    // Process the uploaded file and save data to the database
                    UploadExcelHelper.SaveDataFromExcel(tempFilePath);

                    // Delete the temporary file
                    System.IO.File.Delete(tempFilePath);
                }
            }
            catch (Exception ex)
            {
                // Handle any errors or exceptions
                ViewBag.Message = "Error occurred: " + ex.Message;
            }

            return RedirectToAction("Index");
        }

        /// <summary>
        /// Export excel sheet
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult ExportExcel()
        {
            ExcelPackage package = ExportExcelHelper.ExportExcelData();

            // Set the response headers for file download
            Response.Clear();
            Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            Response.AddHeader("content-disposition", "attachment; filename=ExportedData.xlsx");
            Response.BinaryWrite(package.GetAsByteArray());
            Response.End();

            return RedirectToAction("Index");
        }

    }
}
