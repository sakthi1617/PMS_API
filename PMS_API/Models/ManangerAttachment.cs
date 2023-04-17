using System;
using System.Collections.Generic;

namespace PMS_API.Models
{
    public partial class ManangerAttachment
    {
        public int AttachmentId { get; set; }
        public int? ManagerReviewId { get; set; }
        public byte[]? Attachment { get; set; }
        public bool? IsActive { get; set; }
        public bool? IsDeleted { get; set; }
        public DateTime? CreatedOn { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public virtual ManagerGoalReview? ManagerReview { get; set; }
    }
}
