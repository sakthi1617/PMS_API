using System;
using System.Collections.Generic;

namespace PMS_API.Models
{
    public partial class Weightage
    {
        public int Id { get; set; }
        public int? DepartmentId { get; set; }
        public int? DesignationId { get; set; }
        public int TeamId { get; set; }
        public int? SkillId { get; set; }
        public int? Weightage1 { get; set; }

        public virtual Department? Department { get; set; }
        public virtual Designation? Designation { get; set; }
        public virtual Skill? Skill { get; set; }
        public virtual Team? Team { get; set; }
    }
}
