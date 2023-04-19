using PMS_API.ViewModels;
using System;
using System.Collections.Generic;

namespace PMS_API.Models
{
    public partial class Team
    {
        public Team()
        {
            EmployeeModules = new HashSet<EmployeeModule>();
            Weightages = new HashSet<Weightage>();
        }

        public int TeamId { get; set; }
        public int? DepartmentId { get; set; }
        public string? TeamName { get; set; }

        public virtual Department? Department { get; set; }
        public virtual ICollection<EmployeeModule> EmployeeModules { get; set; }
        public virtual ICollection<Weightage> Weightages { get; set; }
    }
}
