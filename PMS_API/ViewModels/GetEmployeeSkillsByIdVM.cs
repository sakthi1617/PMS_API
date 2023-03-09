namespace PMS_API.ViewModels
{
    public class GetEmployeeSkillsByIdVM
    {
        public int? EmployeeId { get; set; }
        public string? EmpName { get; set; }
        public string? DepartmenName { get; set; }
        public int? DepartmentId { get; set; }
        public string? DesignationName { get; set; }
        public int? DesignationId { get; set; }
        public int? PotentialLevel { get; set; }
        public string? Skill { get; set; }
        public int SkillId { get; set; }
        public int? Level { get; set; }
        public int? Weightage { get; set; }
    }
}
