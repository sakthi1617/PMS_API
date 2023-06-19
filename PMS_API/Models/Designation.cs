using System;
using System.Collections.Generic;

namespace PMS_API.Models
{
    public partial class Designation
    {
        public Designation()
        {
            EmployeeModules = new HashSet<EmployeeModule>();
            Weightages = new HashSet<Weightage>();
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
        public virtual ICollection<Weightage> Weightages { get; set; }
    }
}
