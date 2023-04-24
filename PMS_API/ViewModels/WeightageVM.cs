namespace PMS_API.ViewModels
{
    public class WeightageVM
    {
        public int Id { get; set; }
        public int? DepartmentId { get; set; }
        public int TeamId { get; set; } 
        public int? DesignationId { get; set; }
        public List<int> SkillId { get; set; }
       
    }

    public class DeleteWeightage
    {
        public int? DepartmentId { get; set; }
        public int TeamId { get; set; }
        public int? DesignationId { get; set; }
        public int? SkillId { get; set; }
    }
}
