namespace PMS_API.Models
{
    public class QuestionBank
    {
        public QuestionBank()
        {
            QuestionPapers = new HashSet<QuestionPaper>();
        }
        public int QuestionId { get; set; }
        public int? SkillId { get; set; }
        public string? Question { get; set; }
        public string? OptionA { get; set; }
        public string? OptionB { get; set; }
        public string? OptionC { get; set; }
        public string? OptionD { get; set; }
        public string? CorrectOption { get; set; }
        public int? QuestionLevel { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public int? QuestionType { get; set; }
        public int? SkillLevel { get; set; }
        public int? Marks { get; set; }



        public virtual Skill? Skill { get; set; }
        public virtual QuestionType? QuestionTypeNavigation { get; set; }
        public virtual ICollection<QuestionPaper> QuestionPapers { get; set; }
        public virtual QuestionMarkType? MarksNavigation { get; set; }
        public virtual QuetionLevel? QuestionLevelNavigation { get; set; }
    }
}
