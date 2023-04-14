using System;
using System.Collections.Generic;

namespace PMS_API.Models
{
    public partial class Designation1
    {
        public Designation1()
        {
            EmployeeModules = new HashSet<EmployeeModule>();
        }

        public int DesignationId { get; set; }
        public int? DepartmentId { get; set; }
        public string? DesignationName { get; set; }
        public string? AddBy { get; set; }
        public DateTime? AddTime { get; set; }
        public string? ModifiedBy { get; set; }
        public DateTime? ModifiedTime { get; set; }

        public virtual Department? Department { get; set; }
        public virtual ICollection<EmployeeModule> EmployeeModules { get; set; }
    }
}
