namespace PMS_API.Models
{
    public class QuetionLevel
    {
        public QuetionLevel()
        {
            QuestionBanks = new HashSet<QuestionBank>();
        }

        public int QuetionLevelId { get; set; }
        public string? QuetionLevelName { get; set; }

        public virtual ICollection<QuestionBank> QuestionBanks { get; set; }
    }
}
