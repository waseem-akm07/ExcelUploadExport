using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SampleExcelUploadExport.Helpers.DTOs
{
    public class ContractDto
    {
        public int ID { get; set; }
        public string Client { get; set; }
        public string Single_Master { get; set; }
        public string Joint_Venture { get; set; }
        public string Name { get; set; }
        public string ShortName { get; set; }
        public string ContactNumber { get; set; }
        public Nullable<System.DateTime> StartDate { get; set; }
        public Nullable<System.DateTime> EndDate { get; set; }
        public string ContractManager { get; set; }
        public string TimesheetVeriosnType { get; set; }
    }
}