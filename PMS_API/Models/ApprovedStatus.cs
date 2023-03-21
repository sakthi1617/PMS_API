namespace PMS_API.Models
{
    public partial class ApprovedStatus
    {
        public  ApprovedStatus()
        {
            ResponseMails = new HashSet<ResponseMail>();
        }

        public int ApprovalId { get; set; }
        public int? ReqId { get; set; }
        public string? ApprovedBy { get; set; }
        public string? Comments { get; set; }
        public int? Status { get; set; }
        public bool? Iactive { get; set; }

        public virtual RequestForApproved? Req { get; set; }
        public virtual ICollection<ResponseMail> ResponseMails { get; set; }
    }
}
