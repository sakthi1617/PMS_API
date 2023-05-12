namespace PMS_API.Models
{
    public class QuestionBank
    {
        public int QuestionId { get; set; }
        public int? SkillId { get; set; }
        public string? Question { get; set; }
        public string? OptionA { get; set; }
        public string? OptionB { get; set; }
        public string? OptionC { get; set; }
        public string? OptionD { get; set; }
        public string? CorrectOption { get; set; }
        public string? QuestionLevel { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }

        public virtual Skill? Skill { get; set; }
    }
}
