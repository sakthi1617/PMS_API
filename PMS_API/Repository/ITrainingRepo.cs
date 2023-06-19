using PMS_API.Models;
using PMS_API.SupportModel;
using PMS_API.ViewModels;
using System.Reflection.Metadata;

namespace PMS_API.Repository
{
    public interface ITrainingRepo
    {
        public string AddQuestion(QuestionBankVM questionBank);
        public string UpdateQuestion(int QuestionID,QuestionBankVM questionBank);
        public string DeleteQuestion(int QuestionID);
        public List<QuestionBankVM> GetAllQuestions();
        public List<QuestionBankVM> QuestionsByskillId(int skillId, int level);
        public List<TestQuestionsVM> EmpExamQuestions(int skillId, int EmployeeId);
        public string GenerateQustionPaper(GenerateQustionPaper generate);
        public string SetPassLimit(int QuestionPaperId, int LimitPersentage);
        public List<TestQuestionsVM> GetQustionPaperForExaminer(int QuetionPaperId);
        public dynamic GetQustionPaperForExamer(string EmployeeIdentity,int QuetionPaperId);
        public string EditQuestionPaper(int QuestionPaperId, GenerateQustionPaper generate);
        public List<QuestionPaperIdentity> GetSkillwiseAllQuestionPaper(int skillId);
        public string AssignToEmployee(string EmployeeIdentity, int QuestionpaperID);
        public dynamic ValidateAnswers(List<ValidateVM> validate);
        public TestStatusDetails QuestionPaperStatus(int QuestionpaperId);
        public dynamic QuestionPaperStatusDetail(int QuestionpaperId);       
        public void documentValidation(List<DocumentValidationVM> document);
        public void FindDocumented();
        public void Updatemarks();
        public void ResultNotification();
    }
}
