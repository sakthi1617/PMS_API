using System;
using System.Collections.Generic;

namespace PMS_API.Models
{
    public partial class QuestionType
    {
        public QuestionType()
        {
            QuestionBanks = new HashSet<QuestionBank>();
            QuestionPapers = new HashSet<QuestionPaper>();
        }

        public int TypeId { get; set; }
        public string? QuestionType1 { get; set; }

        public virtual ICollection<QuestionBank> QuestionBanks { get; set; }
        public virtual ICollection<QuestionPaper> QuestionPapers { get; set; }
    }
}
