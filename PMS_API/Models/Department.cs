using Org.BouncyCastle.Utilities.IO;
using PMS_API.ViewModels;
using System;
using System.Collections.Generic;

namespace PMS_API.Models
{
    public partial class Department
    {
        public Department()
        {
            EmployeeModules = new HashSet<EmployeeModule>();
            Weightages = new HashSet<Weightage>();
            Designation1s = new HashSet<Designation1>();
            Teams = new HashSet<Team>();
        }

        public int DepartmentId { get; set; }
        public string? DepartmentName { get; set; }
        public string? AddBy { get; set; }
        public DateTime? AddTime { get; set; }
        public string? ModifiedBy { get; set; }
        public DateTime? ModifiedTime { get; set; }

        public virtual ICollection<EmployeeModule> EmployeeModules { get; set; }
        public virtual ICollection<Weightage> Weightages { get; set; }
        public virtual ICollection<Designation1> Designation1s { get; set; }
        public virtual ICollection<Team> Teams { get; set; }
    }
}
