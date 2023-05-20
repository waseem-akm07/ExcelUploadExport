using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using OfficeOpenXml;
using SampleExcelUploadExport.Models;

namespace SampleExcelUploadExport.Helpers
{
    public static class UploadExcelHelper
    {
        /// <summary>
        /// To save data from excel
        /// </summary>
        /// <param name="filePath"></param>
        public static void SaveDataFromExcel(string filePath)
        {
            string sheetName1 = "ContractBasicInfo";
            string sheetName2 = "LabourCategory";

            try
            {
                // Read Excel file into data table
                DataTable contractData = ParseExcelSheet(filePath, sheetName1);
                DataTable laborCategoryData = ParseExcelSheet(filePath, sheetName2);

                // Insert data into the database tables
               var contracts = InsertContractData(contractData);
               var laborCategory = InsertLabourCategoryData(laborCategoryData, contracts);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Read excel sheet
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="sheetName"></param>
        /// <returns></returns>
        private static DataTable ParseExcelSheet(string filePath, string sheetName)
        {
            DataTable dataTable = new DataTable();

            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            using (ExcelPackage package = new ExcelPackage(new System.IO.FileInfo(filePath)))
            {
                ExcelWorksheet worksheet = package.Workbook.Worksheets[sheetName];

                if (worksheet != null)
                {
                    int rowCount = worksheet.Dimension.Rows;
                    int columnCount = worksheet.Dimension.Columns;

                    // Add columns to DataTable
                    for (int col = 1; col <= columnCount; col++)
                    {
                        var cellValue = worksheet.Cells[1, col].Value;
                        string columnName = cellValue != null ? cellValue.ToString() : $"Column{col}";
                        dataTable.Columns.Add(columnName);
                    }

                    // Add rows to DataTable
                    for (int row = 2; row <= rowCount; row++)
                    {
                        DataRow dataRow = dataTable.NewRow();
                        for (int col = 1; col <= columnCount; col++)
                        {
                            dataRow[col - 1] = worksheet.Cells[row, col].Value?.ToString();
                        }
                        dataTable.Rows.Add(dataRow);
                    }
                }
            }

            return dataTable;
        }

        /// <summary>
        /// Save contract data
        /// </summary>
        /// <param name="data"></param>
        /// <param name="tableName"></param>
        /// <param name="connection"></param>
        /// <param name="transaction"></param>
        private static List<Contract> InsertContractData(DataTable data)
        {
            List<Contract> contractList = new List<Contract>();
            for (int i = 0; i < data.Rows.Count; i++)
            {
                Contract contract = new Contract();
                contract.Client = data.Rows[i]["Client"].ToString();
                contract.Single_Master = data.Rows[i]["Single_Master"].ToString();
                contract.Joint_Venture = data.Rows[i]["Joint_Venture"].ToString();
                contract.Name = data.Rows[i]["Name"].ToString();
                contract.ShortName = data.Rows[i]["ShortName"].ToString();
                contract.ContactNumber = data.Rows[i]["ContactNumber"].ToString();
                contract.StartDate = Convert.ToDateTime(data.Rows[i]["StartDate"]);
                contract.EndDate = Convert.ToDateTime(data.Rows[i]["EndDate"]);
                contract.ContractManager = data.Rows[i]["ContractManager"].ToString();
                contract.TimesheetVeriosnType = data.Rows[i]["TimesheetVersionType"].ToString();

                contractList.Add(contract);
            }

            using (var db = new ExcelEntities())
            {
                db.Contracts.AddRange(contractList);
                db.SaveChanges();
            }
            return contractList;
        }

        /// <summary>
        /// Save LaborCategory data
        /// </summary>
        /// <param name="data"></param>
        /// <param name="contracts"></param>
        /// <returns></returns>
        private static List<LabourCategory> InsertLabourCategoryData(DataTable data, List<Contract> contracts)
        {
            List<LabourCategory> labourCategoryList = new List<LabourCategory>();
            foreach (var contract in contracts)
            {
                for (int i = 0; i < data.Rows.Count; i++)
                {
                    LabourCategory labourCategory = new LabourCategory();
                    labourCategory.CommonLabourCategory = data.Rows[i]["CommonLabourCategory"].ToString();
                    labourCategory.ContractName = data.Rows[i]["ContractName"].ToString();
                    labourCategory.DisplayName = data.Rows[i]["DisplayName"].ToString();
                    labourCategory.Contract_ID = contract.ID;
                    labourCategory.EEO = data.Rows[i]["EEO"].ToString();
                    labourCategory.ShortName = data.Rows[i]["ShortName"].ToString();

                    labourCategoryList.Add(labourCategory);
                }
            }

            using (var db = new ExcelEntities())
            {
                db.LabourCategories.AddRange(labourCategoryList);
                db.SaveChanges();
            }
            return labourCategoryList;
        }
    }
}