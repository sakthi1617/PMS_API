namespace PMS_API.Models
{
    public partial class ResponseMail
    {
        public int ResId { get; set; }
        public int? ReqId { get; set; }
        public int? ApprovalId { get; set; }
        public string? MailFrom { get; set; }
        public string? MailTo { get; set; }
        public string? MailCc { get; set; }
        public bool? IsDeliverd { get; set; }
        public DateTime? DeliverdAt { get; set; }

        public virtual ApprovedStatus? Approval { get; set; }
        public virtual RequestForApproved? Req { get; set; }
    }
}
