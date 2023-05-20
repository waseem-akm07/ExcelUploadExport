using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SampleExcelUploadExport.Helpers.DTOs
{
    public class LabourCategoryDto
    {
        public int ID { get; set; }
        public string ContractName { get; set; }
        public string CommonLabourCategory { get; set; }
        public string DisplayName { get; set; }
        public string ShortName { get; set; }
        public string EEO { get; set; }
    }
}