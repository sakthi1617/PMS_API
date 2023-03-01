using PMS_API.Models;
using PMS_API.ViewModels;

namespace PMS_API.Repository
{
    public interface IOrganizationRepo
    {
        public void AddEmployee(EmployeeVM model);
        public void AddSkill(Skillset model);

        public List<EmployeeModule> EmployeeList();
        public List<Skillset> SkilsList();

        public void Save();
    }
}
