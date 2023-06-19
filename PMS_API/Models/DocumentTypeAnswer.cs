namespace PMS_API.Models
{
    public class DocumentTypeAnswer
    {
        public int DocumentId { get; set; }
        public string? EmployeeIdentity { get; set; }
        public int? SkillId { get; set; }
        public int? QuestionPaperId { get; set; }
        public DateTime? AssigndAt { get; set; }
        public int? QuestionNumber { get; set; }
        public string? DocumentUrl { get; set; }
        public DateTime? SubmittedAt { get; set; }
        public int? MaximumMark { get; set; }
        public int? MarksObtained { get; set; }
        public DateTime? ValidatedAt { get; set; }
        public bool? IsNotified { get; set; }
        public bool? IsValidated { get; set; }
        public bool? IsUpdated { get; set; }
    }
}
