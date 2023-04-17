namespace PMS_API.ViewModels
{
  
        public class TestEmployeeList
        {
            public int? FirstLevelReportingManager { get; set; }
            public string FirstLevelReportingManagerName { get; set; }
            public int? SecondLevelReportingManager { get; set; }
            public string SecondLevelReportingManagerName { get; set; }
            public TestEmployeeVM EmployeeVMs { get; set; }
        }
    
}
