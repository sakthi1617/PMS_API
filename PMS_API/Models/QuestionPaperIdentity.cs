using System;
using System.Collections.Generic;

namespace PMS_API.Models
{
    public partial class QuestionPaperIdentity
    {
        public QuestionPaperIdentity()
        {
            QuestionPapers = new HashSet<QuestionPaper>();
        }

        public int QuestionPaperId { get; set; }
        public int? SkillId { get; set; }
        public string? QuestionPaperName { get; set; }
        public int? SkillLevel { get; set; }
        public int? QuestionLevel { get; set; }
        public int? NumberOfQuestions { get; set; }
        public string? QuestionMarkType { get; set; }
        public int? MaximumMark { get; set; }
        public DateTime? CreatedOn { get; set; }
        public bool? IsDeleted { get; set; }
        public int? PassPercentage { get; set; }
        public virtual ICollection<QuestionPaper> QuestionPapers { get; set; }
    }

}
