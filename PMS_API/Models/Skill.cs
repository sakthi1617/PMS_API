﻿using System;
using System.Collections.Generic;

namespace PMS_API.Models
{
    public partial class Skill
    {
        public Skill()
        {
            Weightages = new HashSet<Weightage>();
            UserLevels = new HashSet<UserLevel>();
        }

        public int SkillId { get; set; }
        public string? SkillName { get; set; }

        public virtual ICollection<Weightage> Weightages { get; set; }
        public virtual ICollection<UserLevel> UserLevels { get; set; }
    }
}
