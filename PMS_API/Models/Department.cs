using System;
using System.Collections.Generic;

namespace PMS_API.Models
{
    public partial class Department
    {
        public Department()
        {
            EmployeeModules = new HashSet<EmployeeModule>();
            Skillsets = new HashSet<Skillset>();
        }

        public int DesignationId { get; set; }
        public string? DesignationName { get; set; }

        public virtual ICollection<EmployeeModule> EmployeeModules { get; set; }
        public virtual ICollection<Skillset> Skillsets { get; set; }
    }
}
