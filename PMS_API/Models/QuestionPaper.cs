using System;
using System.Collections.Generic;

namespace PMS_API.Models
{
    public partial class QuestionPaper
    {
        public int Id { get; set; }
        public int? QuestionPaperId { get; set; }
        public int? QuestionId { get; set; }
        public string? Question { get; set; }
        public string? OptionA { get; set; }
        public string? OptionB { get; set; }
        public string? OptionC { get; set; }
        public string? OptionD { get; set; }
        public int? QuestionType { get; set; }

        public virtual QuestionBank? QuestionNavigation { get; set; }
        public virtual QuestionPaperIdentity? QuestionPaperNavigation { get; set; }
        public virtual QuestionType? QuestionTypeNavigation { get; set; }
    }

}
