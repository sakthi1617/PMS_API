using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace PMS_API.Models
{
    public partial class EmployeeAttachment
    {
       
        public int? AttachmentId { get; set; }
        public int? EmpReviewId { get; set; }
        public byte[]? Attachment { get; set; }
        public bool? IsActive { get; set; }
        public bool? IsDeleted { get; set; }
        public DateTime? CreatedOn { get; set; }
        public DateTime? ModifiedOn { get; set; }

        public virtual EmployeeGoalReview? EmpReview { get; set; }
    }
}
