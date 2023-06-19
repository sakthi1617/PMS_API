using PMS_API.ViewModels;

namespace PMS_API.SupportModel
{
    public class TestStatusDetails
    {
        public int AssignedEmployees { get; set; }
        public int NotOpend { get; set; }
        public int Inprogress { get; set; }
        public int Completed { get; set; }
        public int Validated { get; set; }
        public int PassedEmployee  { get; set; }
        public int FailedEmployee  { get; set; }

    }

    public class EmpList
    {
        public List<EmployeeDetail>? Assigend { get; set; }
        public List<EmployeeDetail>? NotOpened { get; set; }
        public List<EmployeeDetail>? InProgress { get; set; }
        public List<EmployeeDetail>? Completed { get; set; }
        public List<EmployeeDetail>? Passed { get; set; }
        public List<EmployeeDetail>? Failed { get; set; }
       
    }
}
