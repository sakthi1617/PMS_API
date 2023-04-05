namespace PMS_API.Models
{
    public partial class RequestForApproved
    {
        public RequestForApproved()
        {
            ResponseEmails = new HashSet<ResponseEmail>();
        }

        public int ReqId { get; set; }
        public int? EmployeeId { get; set; }
        public int? RequestCreatedById { get; set; }
        public string? RequestCreatedBy { get; set; }
        public DateTime? RequestCreatedAt { get; set; }
        public string? Reason { get; set; }
        public string? Comments { get; set; }
        public string? ModifiedBy { get; set; }
        public DateTime? ModifiedAt { get; set; }
        public bool? IsActivated { get; set; }
        public bool? IsDeliverd { get; set; }
        public int? Skillid { get; set; }

        public virtual EmployeeModule? Employee { get; set; }
        public virtual ManagersTbl? RequestCreatedByNavigation { get; set; }
        public virtual Skill? Skill { get; set; }
        public virtual ICollection<ResponseEmail> ResponseEmails { get; set; }
    }
}
