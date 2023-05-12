using PMS_API.Data;
using PMS_API.Models;
using PMS_API.Repository;
using PMS_API.ViewModels;
using System.Collections;
using System.Collections.Generic;

namespace PMS_API.Services
{
    public class Trainingservice : ITrainingRepo
    {
        private readonly PMSContext _context;

        public Trainingservice(PMSContext context)
        {
            _context = context; 
        }

        
        public string AddQuestion(QuestionBankVM questionBank)
        {
            var Dept = _context.Skills.Where(x => x.SkillId == questionBank.SkillId).FirstOrDefault();    
            if (Dept != null) 
            {
                QuestionBank question = new QuestionBank();
                question.SkillId = questionBank.SkillId;
                question.Question = questionBank.Question + "?";
                question.OptionA = questionBank.OptionA;
                question.OptionB = questionBank.OptionB;
                question.OptionC = questionBank.OptionC;
                question.OptionD = questionBank.OptionD;
                question.CorrectOption = questionBank.CorrectOption;
                question.QuestionLevel = questionBank.QuestionLevel;
                question.CreatedAt = DateTime.Now;
                _context.QuestionBanks.Add(question);
                _context.SaveChanges();
                return "Success";
            }
            return "Error";
        }
        
        public string UpdateQuestion(int QuestionID, QuestionBankVM questionBank)
        {
            var question = _context.QuestionBanks.Where(x => x.QuestionId == QuestionID).FirstOrDefault();
            if (question != null)
            {
                question.SkillId = questionBank.SkillId;
                question.Question = questionBank.Question ;
                question.OptionA = questionBank.OptionA;
                question.OptionB = questionBank.OptionB;
                question.OptionC = questionBank.OptionC;
                question.OptionD = questionBank.OptionD;
                question.CorrectOption = questionBank.CorrectOption;
                question.QuestionLevel = questionBank.QuestionLevel;
                question.UpdatedAt = DateTime.Now;
                _context.QuestionBanks.Update(question);
                _context.SaveChanges();
                return "Success";
            }
            return "Error"; 

        }

        public string DeleteQuestion(int QuestionID)
        {
            var delQue = _context.QuestionBanks.Where(x => x.QuestionId == QuestionID).FirstOrDefault();
            if(delQue!= null)
            {
                _context.QuestionBanks.Remove(delQue);
                _context.SaveChanges();
                return "Success";
            }
            return"Error";  
        }

        public List<QuestionBankVM> GetAllQuestions()
        {
            List<QuestionBankVM> allQuestions = new List<QuestionBankVM>();
            var a = _context.QuestionBanks.ToList();
            if(a.Count > 0)
            {
                foreach (var item in a)
                {
                    QuestionBankVM data = new QuestionBankVM(); 
                    data.SkillId= item.SkillId;
                    data.Question = item.Question;
                    data.OptionA = item.OptionA;
                    data.OptionB = item.OptionB;
                    data.OptionC = item.OptionC;
                    data.OptionD = item.OptionD;
                    data.CorrectOption = item.CorrectOption;    
                   data.QuestionLevel= item.QuestionLevel;

                    allQuestions.Add(data);
                }
                return allQuestions;
            }
            return allQuestions;   
        }

        public List<QuestionBankVM> QuestionsByskillId(int skillId, string level)
        {
            List<QuestionBankVM> allQuestions = new List<QuestionBankVM>();
            var a = _context.QuestionBanks.Where(x => x.SkillId == skillId && x.QuestionLevel ==level ).ToList();
            if (a.Count > 0)
            {
                foreach (var item in a)
                {
                    QuestionBankVM data = new QuestionBankVM();
                    data.SkillId = item.SkillId;
                    data.Question = item.Question;
                    data.OptionA = item.OptionA;
                    data.OptionB = item.OptionB;
                    data.OptionC = item.OptionC;
                    data.OptionD = item.OptionD;
                    data.CorrectOption = item.CorrectOption;
                    data.QuestionLevel = item.QuestionLevel;

                    allQuestions.Add(data);
                }
                return allQuestions;
            }
            return allQuestions;
        }

        public List<TestQuestionsVM> EmpExamQuestions(int EmployeeId, int skillId )
        {
            var a = _context.UserLevels.Where( x => x.EmployeeId == EmployeeId && x.SkillId == skillId).FirstOrDefault();
            string Level = "Low";
            if(a != null ) 
            {
                switch (a.Level)
                {
                   case 0:
                        Level = "Low";
                        break;
                    case 1:
                        Level = "Low";
                        break;
                    case 2:
                        Level = "Medium";
                        break;
                    case 3:
                        Level = "High";
                        break;
                    case 4:
                        Level = "High";
                        break;
                }
                var Questionslists = Questions(skillId, Level);
                return Questionslists;
            }
            var Questionslists1 = Questions(skillId, Level);
            return Questionslists1;
        }

        public List<TestQuestionsVM> Questions(int skillId, string qLevel)
        {
            List<TestQuestionsVM> list = new List<TestQuestionsVM>();
            List<TestQuestionsVM> selectedItems = new List<TestQuestionsVM>();
            var a = _context.QuestionBanks.Where( x => x.SkillId == skillId && x.QuestionLevel == qLevel).ToList();
           
            

            if (a.Count > 0 )
            {
                

                foreach ( var x in a )
                {
                    TestQuestionsVM test = new TestQuestionsVM();
                    test.QuestionId = x.QuestionId;
                    test.Question = x.Question;
                    test.OptionA = x.OptionA;
                    test.OptionB = x.OptionB;
                    test.OptionC = x.OptionC;
                    test.OptionD = x.OptionD;
                    list.Add(test);
                   // return list;    
                }

                int count = list.Count;
                Random rand = new Random();
                List<int> randomnumbers = new List<int>();
                while (randomnumbers.Count < 5)
                {
                    randomnumbers.Add(rand.Next(count));
                }

               
                foreach (int index in randomnumbers)
                {
                    selectedItems.Add(list[index]);
                }

            }
            return selectedItems;
        }

        public dynamic ValidateAnswers(List<ValidateVM> validate)
        {
            List<int> correctAnswers = new List<int>(); 
            List<int> WorngAnswers = new List<int>(); 
          

         
           if(validate.Count> 0)
            {
                foreach(var item in validate)
                {
                    var a = _context.QuestionBanks.Where( x => x.QuestionId == item.QuestionId ).FirstOrDefault();
                    if (a != null)
                    {
                        if (a.CorrectOption == item.Answer)
                        {
                            correctAnswers.Add(item.QuestionId);
                        }
                        else
                        {
                            WorngAnswers.Add(item.QuestionId);
                        }
                       
                    }
                   
                }
                var result = new MyLists { Correct = correctAnswers, Wrong = WorngAnswers };
                return result;
            }
            return correctAnswers;
        }
    }
}
