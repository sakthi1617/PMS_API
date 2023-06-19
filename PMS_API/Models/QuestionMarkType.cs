namespace PMS_API.Models
{
    public class QuestionMarkType
    {
        public QuestionMarkType()
        {
            QuestionBanks = new HashSet<QuestionBank>();
        }

        public int QuestionMarkTypeId { get; set; }
        public string? QuestionMarkTypeName { get; set; }

        public virtual ICollection<QuestionBank> QuestionBanks { get; set; }
    }
}
