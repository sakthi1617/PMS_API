using PMS_API.ViewModels;
using System;
using System.Collections.Generic;

namespace PMS_API.Models
{
    public partial class Skill
    {
        public Skill()
        {
            RequestForApproveds = new HashSet<RequestForApproved>();
            QuestionBanks = new HashSet<QuestionBank>();
            UserLevels = new HashSet<UserLevel>();
            Weightages = new HashSet<Weightage>();
        }

        public int SkillId { get; set; }
        public string? SkillName { get; set; }

        public virtual ICollection<RequestForApproved> RequestForApproveds { get; set; }

        public virtual ICollection<QuestionBank> QuestionBanks { get; set; }
        public virtual ICollection<UserLevel> UserLevels { get; set; }
        public virtual ICollection<Weightage> Weightages { get; set; }
    }
}
