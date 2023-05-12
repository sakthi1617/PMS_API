using PMS_API.Models;
using PMS_API.ViewModels;

namespace PMS_API.Repository
{
    public interface ISkillRepo
    {
        public void AddSkillWeightage(WeightageVM weightage);
        public void RemoveSkillWeightage(DeleteWeightage weightage);
        public string AddAdditionalSkills(UserLevelVM level);
        public IQueryable<GetEmployeeSkillsByIdVM> GetEmployeeSkillsById(string EmployeeIdentity);
        public void AddSkill(SkillsVM model);
        public string UpdateSkill(int id, SkillsVM skill);
        public string DeleteSkill(int id);
        public List<Skill> SkilsList();
        public List<Weightage> SkillbyDepartmentID(int DeptId , int DesigId , int teamid);
        public string UpdateSkillWeightages(int skillId, string employeeIdentity, int weightage);
        public dynamic ReqForUpdateLvl(string employeeIdentity, int SklID, string descrip, string rea, IFormFileCollection fiels);
        public string LevlelApprovedSuccess(int reqid, bool status); 
        public dynamic UserLevelDecrement(string EmployeeIdentity, int skillId);
        public void Save();
    }
}
