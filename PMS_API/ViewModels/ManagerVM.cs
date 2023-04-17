namespace PMS_API.ViewModels
{
    public class ManagerVM
    {
        public int? EmployeeId { get; set; }
        public string EmployeeName { get; set; }
        public int? FirstLevelManagerId { get; set; }
        public string FirstLevelManagerName { get; set; }
        public int? secondLevelManagerId { get; set; }
        public string SecondLevelManagerName { get; set; }
    }
}
