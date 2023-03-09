using System;
using System.Collections.Generic;

namespace PMS_API.Models
{
    public partial class Role
    {
        public Role()
        {
            EmployeeModules = new HashSet<EmployeeModule>();
        }

        public int RollId { get; set; }
        public string? RollName { get; set; }

        public virtual ICollection<EmployeeModule> EmployeeModules { get; set; }
    }
}
