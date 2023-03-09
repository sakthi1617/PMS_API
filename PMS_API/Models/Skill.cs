using System;
using System.Collections.Generic;

namespace PMS_API.Models
{
    public partial class Skill
    {
        public Skill()
        {
            Weightages = new HashSet<Weightage>();
        }

        public int SkillId { get; set; }
        public string? SkillName { get; set; }

        public virtual ICollection<Weightage> Weightages { get; set; }
    }
}
