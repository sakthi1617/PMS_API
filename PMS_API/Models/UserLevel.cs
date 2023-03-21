using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PMS_API.Models
{
    public partial class UserLevel
    {
        [Key]
        public int? Id { get; set; }
        public int? EmployeeId { get; set; }
        public int? SkillId { get; set; }
        public int? Level { get; set; }
        public int? Weightage { get; set; }

        public virtual EmployeeModule? Employee { get; set; }

        public virtual Skill? Skill { get; set; }
    }
}
