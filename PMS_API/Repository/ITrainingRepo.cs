using PMS_API.ViewModels;

namespace PMS_API.Repository
{
    public interface ITrainingRepo
    {
        public string AddQuestion(QuestionBankVM questionBank);
        public string UpdateQuestion(int QuestionID,QuestionBankVM questionBank);

        public string DeleteQuestion(int QuestionID);

        public List<QuestionBankVM> GetAllQuestions();

        public List<QuestionBankVM> QuestionsByskillId(int skillId, string level);

        public List<TestQuestionsVM> EmpExamQuestions(int skillId, int EmployeeId);

        public dynamic ValidateAnswers(List<ValidateVM> validate);


    }
}
