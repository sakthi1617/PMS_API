﻿namespace PMS_API.Models
{
    public partial class RequestForApproved
    {
        public RequestForApproved()
        {
            ApprovedStatuses = new HashSet<ApprovedStatus>();
            ResponseMails = new HashSet<ResponseMail>();
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

        public virtual EmployeeModule? Employee { get; set; }
        public virtual ManagersTbl? RequestCreatedByNavigation { get; set; }
        public virtual ICollection<ApprovedStatus> ApprovedStatuses { get; set; }
        public virtual ICollection<ResponseMail> ResponseMails { get; set; }
    }
}
