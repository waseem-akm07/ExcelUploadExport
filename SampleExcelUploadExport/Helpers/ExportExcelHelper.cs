using OfficeOpenXml;
using SampleExcelUploadExport.Helpers.DTOs;
using SampleExcelUploadExport.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Web;

namespace SampleExcelUploadExport.Helpers
{
    public static class ExportExcelHelper
    {
        /// <summary>
        /// Export data into excel
        /// </summary>
        /// <returns></returns>
        public static ExcelPackage ExportExcelData()
        {
            DataTable contractData;
            DataTable laborCategoryData;

            // Retrieve the data from the database tables
            contractData = GetContractData();
            laborCategoryData = GetLabourData();

            // Generate the Excel file
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            var package = new ExcelPackage();
            var contractsSheet = package.Workbook.Worksheets.Add("ContractBasicInfo");
            var laborCategoriesSheet = package.Workbook.Worksheets.Add("LaborCategory");

            // Populate the Contract Basic Info sheet
            contractsSheet.Cells.LoadFromDataTable(contractData, true);

            // Populate the Labor Category sheet
            laborCategoriesSheet.Cells.LoadFromDataTable(laborCategoryData, true);

            return package;
        }

        /// <summary>
        /// Get contract data
        /// </summary>
        /// <returns></returns>
        private static DataTable GetContractData()
        {
            List<ContractDto> contractList = new List<ContractDto>();
            using (var db = new ExcelEntities())
            {
                contractList = (from con in db.Contracts 
                                select new ContractDto 
                                {
                                    ID = con.ID,
                                    Name = con.Name,
                                    Client = con.Client,
                                    EndDate = con.EndDate,
                                    StartDate = con.StartDate,
                                    ShortName = con.ShortName,
                                    Single_Master = con.Single_Master,
                                    Joint_Venture = con.Joint_Venture,
                                    ContactNumber = con.ContactNumber,
                                    ContractManager = con.ContractManager,
                                    TimesheetVeriosnType = con.TimesheetVeriosnType
                                }).ToList();
            }

            if (contractList != null && contractList.Count() > 0)
            {
                DataTable dataTable = new DataTable(typeof(ContractDto).Name);
                //Get all the properties
                PropertyInfo[] Props = typeof(ContractDto).GetProperties(BindingFlags.Public | BindingFlags.Instance);

                foreach (PropertyInfo prop in Props)
                {
                    //Setting column names as Property names
                    dataTable.Columns.Add(prop.Name);
                }

                foreach (var item in contractList)
                {
                    var values = new object[Props.Length];
                    for (int i = 0; i < Props.Length; i++)
                    {
                        //inserting property values to datatable rows
                        values[i] = Props[i].GetValue(item, null);
                    }
                    dataTable.Rows.Add(values);
                }

                return dataTable;
            }
            else
            {
                return new DataTable();
            }
        }

        /// <summary>
        /// Get Labour data
        /// </summary>
        /// <returns></returns>
        private static DataTable GetLabourData()
        {
            List<LabourCategoryDto> labourCategories = new List<LabourCategoryDto>();
            using (var db = new ExcelEntities())
            {
                labourCategories = (from labour in db.LabourCategories
                                    select new LabourCategoryDto
                                    {
                                        ID = labour.ID,
                                        EEO = labour.EEO,
                                        ShortName = labour.ShortName,
                                        DisplayName = labour.DisplayName,
                                        ContractName = labour.ContractName,
                                        CommonLabourCategory = labour.CommonLabourCategory,
                                    }).ToList();
            }

            if (labourCategories != null && labourCategories.Count() > 0)
            {
                DataTable dataTable = new DataTable(typeof(LabourCategoryDto).Name);
                //Get all the properties
                PropertyInfo[] Props = typeof(LabourCategoryDto).GetProperties(BindingFlags.Public | BindingFlags.Instance);

                foreach (PropertyInfo prop in Props)
                {
                    //Setting column names as Property names
                    dataTable.Columns.Add(prop.Name);
                }

                foreach (var item in labourCategories)
                {
                    var values = new object[Props.Length];
                    for (int i = 0; i < Props.Length; i++)
                    {
                        //inserting property values to datatable rows
                        values[i] = Props[i].GetValue(item, null);
                    }
                    dataTable.Rows.Add(values);
                }

                return dataTable;
            }
            else
            {
                return new DataTable();
            }
        }
    }
}