using System;
using System.Collections.Generic;

namespace PMS_API.Models
{
    public partial class TimeSettingTbl
    {
        public int Id { get; set; }
        public int? ManagerReviewDay { get; set; }
        public int? EmployeeReviewDay { get; set; }
    }
}
