namespace PMS_API.Models
{
    public partial class ResponseEmail
    {
        public int ResponseId { get; set; }
        public int? ReqId { get; set; }
        public bool? Status { get; set; }
        public int? EmployeeId { get; set; }
        public string? Employeename { get; set; }
        public int? Skillid { get; set; }
        public string? SkillName { get; set; }
        public string? FirstLvlManagerName { get; set; }
        public string? FirstlvlManagerMail { get; set; }
        public string? SecondlvlManagerName { get; set; }
        public string? SecondlvlManagerMail { get; set; }
        public string? Employeemail { get; set; }
        public bool? IsDeliverd { get; set; }
        public DateTime? DeliverdAt { get; set; }
        public bool? IsNotified { get; set; }
        public DateTime? NotifiedAt { get; set; }
        public bool? IsUpdated { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public bool? IsActive { get; set; }

        public virtual RequestForApproved? Req { get; set; }
    }
}
