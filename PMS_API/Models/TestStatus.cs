namespace PMS_API.Models
{
    public class TestStatus
    {
        public int StatusId { get; set; }
        public string? EmployeeIdentity { get; set; }
        public int? QuestionPaperId { get; set; }
        public bool? IsAssigned { get; set; }
        public bool? IsOpened { get; set; }
        public bool? Inprogress { get; set; }
        public bool? IsCompleted { get; set; }
        public bool? IsValidated { get; set; }
        public int? MaximumMark { get; set; }
        public int? MarksObtained { get; set; }
        public string? TestStatus1 { get; set; }
        public DateTime? AssignedAt { get; set; }
        public DateTime? OpenedAt { get; set; }
        public DateTime? CompletedAt { get; set; }
        public DateTime? ValidatedAt { get; set; }
        public bool? ItHaveADocument { get; set; }
        public bool? ResulDeleiverd { get; set; }
    }
}
