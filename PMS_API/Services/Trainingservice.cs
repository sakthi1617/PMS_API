using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using MimeKit;
using Org.BouncyCastle.Asn1.Ocsp;
using PMS_API.Data;
using PMS_API.Models;
using PMS_API.Repository;
using PMS_API.SupportModel;
using PMS_API.ViewModels;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net.Mail;
using System.Net;
using System.Runtime.InteropServices;
using static System.Net.WebRequestMethods;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Net.Mime;
using System.Text;
using Org.BouncyCastle.Pqc.Crypto.Lms;
using System.Reflection.Metadata;
using Hangfire.MemoryStorage.Database;
using System.Collections.Immutable;

namespace PMS_API.Services
{
    public class Trainingservice : ITrainingRepo
    {
        private readonly PMSContext _context;
        private readonly IEmailService _emailservice;
        private readonly IWebHostEnvironment _hostingEnvironment;


        public Trainingservice(PMSContext context, IEmailService emailservice, IWebHostEnvironment hostingEnvironment)
        {
            _context = context;
            _emailservice = emailservice;
            _hostingEnvironment = hostingEnvironment;
        }
        public string AddQuestion(QuestionBankVM questionBank)
        {
            var Dept = _context.Skills.Where(x => x.SkillId == questionBank.SkillId).FirstOrDefault();
            if (Dept != null)
            {
                QuestionBank question = new QuestionBank();
                question.SkillId = questionBank.SkillId;
                question.Question = questionBank.Question;
                question.OptionA = questionBank.OptionA;
                question.OptionB = questionBank.OptionB;
                question.OptionC = questionBank.OptionC;
                question.OptionD = questionBank.OptionD;
                question.CorrectOption = questionBank.CorrectOption;
                question.QuestionLevel = questionBank.QuestionLevel;
                question.CreatedAt = DateTime.Now;
                question.QuestionType = questionBank.QuestionType;
                question.Marks = questionBank.Marks;
                question.SkillLevel = questionBank.SkillLevel;
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
                question.Question = questionBank.Question;
                question.OptionA = questionBank.OptionA;
                question.OptionB = questionBank.OptionB;
                question.OptionC = questionBank.OptionC;
                question.OptionD = questionBank.OptionD;
                question.CorrectOption = questionBank.CorrectOption;
                question.QuestionLevel = questionBank.QuestionLevel;
                question.QuestionType = questionBank.QuestionType;
                question.Marks = questionBank.Marks;
                question.SkillLevel = questionBank.SkillLevel;
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
            if (delQue != null)
            {
                _context.QuestionBanks.Remove(delQue);
                _context.SaveChanges();
                return "Success";
            }
            return "Error";
        }
        public List<QuestionBankVM> GetAllQuestions()
        {
            
            List<QuestionBankVM> allQuestions = new List<QuestionBankVM>();
            var a = _context.QuestionBanks.ToList();
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
                    data.SkillLevel = item.SkillLevel;
                    data.Marks = item.Marks;


                    allQuestions.Add(data);
                }
                return allQuestions;
            }
            return allQuestions;
        }
        public List<QuestionBankVM> QuestionsByskillId(int skillId, int level)
        {
            List<QuestionBankVM> allQuestions = new List<QuestionBankVM>();
            var a = _context.QuestionBanks.Where(x => x.SkillId == skillId && x.QuestionLevel == level).ToList();
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
                    data.SkillLevel = item.SkillLevel;
                    data.Marks = item.Marks;

                    allQuestions.Add(data);
                }
                return allQuestions;
            }
            return allQuestions;
        }
        public List<TestQuestionsVM> EmpExamQuestions(int EmployeeId, int skillId)
        {
            var a = _context.UserLevels.Where(x => x.EmployeeId == EmployeeId && x.SkillId == skillId).FirstOrDefault();
            int Level = 1;
            if (a != null)
            {
                switch (a.Level)
                {
                    case 0:
                        Level = 1;
                        break;
                    case 1:
                        Level = 1;
                        break;
                    case 2:
                        Level = 2;
                        break;
                    case 3:
                        Level = 3;
                        break;
                    case 4:
                        Level = 3;
                        break;
                }
                var Questionslists = Questions(skillId, Level);
                return Questionslists;
            }
            var Questionslists1 = Questions(skillId, Level);
            return Questionslists1;
        }
        public string GenerateQustionPaper(GenerateQustionPaper generate)
        {
            List<TestQuestionsVM> AllQuestions = new List<TestQuestionsVM>();
            List<TestQuestionsVM> SelectedQuestions = new List<TestQuestionsVM>();
            int totalmark = 0;
            dynamic a = null;

            if (generate.quetionMarkType == 0)
            {
                a = _context.QuestionBanks.Where(x => x.SkillId.Equals(generate.SkillId) && x.SkillLevel.Equals(generate.Skilllevel) && x.QuestionLevel.Equals(generate.Questionlevel)).ToList();
            }
            else
            {
                a = _context.QuestionBanks.Where(x => x.SkillId.Equals(generate.SkillId) && x.SkillLevel.Equals(generate.Skilllevel) && x.QuestionLevel.Equals(generate.Questionlevel) && x.Marks.Equals(generate.quetionMarkType)).ToList();
            }
            if (a.Count > 0)
            {
                foreach (var x in a)
                {
                    TestQuestionsVM test = new TestQuestionsVM();
                    test.QuestionId = x.QuestionId;
                    test.Question = x.Question;
                    test.OptionA = x.OptionA;
                    test.OptionB = x.OptionB;
                    test.OptionC = x.OptionC;
                    test.OptionD = x.OptionD;
                    test.Marks = x.Marks;
                    test.QuestionType = x.QuestionType;
                    AllQuestions.Add(test);
                }
                int count = AllQuestions.Count;
                Random rand = new Random();
                List<int> randomnumbers = new List<int>();
                while (randomnumbers.Count < generate.NumberofQuestions)
                {
                    randomnumbers.Add(rand.Next(count));
                }
                foreach (int index in randomnumbers)
                {
                    SelectedQuestions.Add(AllQuestions[index]);
                    totalmark += (int)AllQuestions[index].Marks;
                }
            }

            QuestionPaperIdentity identity = new QuestionPaperIdentity();


            identity.QuestionPaperName = generate.QuetionPaperName;
            identity.SkillId = generate.SkillId;
            identity.SkillLevel = generate.Skilllevel;
            identity.QuestionLevel = generate.Questionlevel;
            identity.NumberOfQuestions = generate.NumberofQuestions;
            identity.MaximumMark = totalmark;
            identity.QuestionMarkType = _context.QuestionMarkTypes.FirstOrDefault(c => c.QuestionMarkTypeId == generate.quetionMarkType).QuestionMarkTypeName;
            identity.CreatedOn = DateTime.Now;
            identity.IsDeleted = false;
            _context.QuestionPaperIdentities.Add(identity);
            _context.SaveChanges();
            var Qid = identity.QuestionPaperId;

            foreach (var item in SelectedQuestions)
            {
                QuestionPaper paper = new QuestionPaper();
                paper.QuestionPaperId = Qid;
                paper.QuestionId = item.QuestionId;
                paper.Question = item.Question;
                paper.OptionA = item.OptionA;
                paper.OptionB = item.OptionB;
                paper.OptionC = item.OptionC;
                paper.OptionD = item.OptionD;
                paper.QuestionType = item.QuestionType;
                _context.QuestionPapers.Add(paper);
                _context.SaveChanges();
            }

            return "ok";
        }

        public string SetPassLimit(int QuestionPaperId, int LimitPersentage)
        {
            var data = _context.QuestionPaperIdentities.Where(x => x.QuestionPaperId == QuestionPaperId && x.IsDeleted != true).FirstOrDefault();
            if(data != null)
            {
                data.PassPercentage= LimitPersentage;
                _context.QuestionPaperIdentities.Update(data);
                _context.SaveChanges();
                return "Pass Percentage Updated";
            }
            return "Error";
        }
        public List<QuestionPaperIdentity> GetSkillwiseAllQuestionPaper(int skillId)
        {
            return _context.QuestionPaperIdentities.Where(x => x.SkillId == skillId).ToList();
        }
        public List<TestQuestionsVM> GetQustionPaperForExaminer(int QuetionPaperId)
        {
            var a = _context.QuestionPaperIdentities.Where(x => x.QuestionPaperId == QuetionPaperId).FirstOrDefault();
            List<TestQuestionsVM> list = new List<TestQuestionsVM>();

            if (a != null)
            {
                var questions = _context.QuestionPapers.Where(x => x.QuestionPaperId == a.QuestionPaperId).ToList();

                if (questions.Count > 0)
                {
                    foreach (var items in questions)
                    {
                        TestQuestionsVM test = new TestQuestionsVM();
                        test.QuestionPaperId = a.QuestionPaperId;
                        test.QuestionId = items.QuestionId;
                        test.Question = items.Question;
                        test.OptionA = items.OptionA;
                        test.OptionB = items.OptionB;
                        test.OptionC = items.OptionC;
                        test.OptionD = items.OptionD;
                        test.QuestionType = items.QuestionType;
                        list.Add(test);
                    }
                }
            }
            return list;
        }
        public dynamic GetQustionPaperForExamer(string EmployeeIdentity, int QuestionPaperId)
        {
            DocumentNotification(EmployeeIdentity);
            List<TestQuestionsVM> list = new List<TestQuestionsVM>();
            var ifAssigned = _context.TestStatuses.Where(x => x.EmployeeIdentity == EmployeeIdentity && x.QuestionPaperId == QuestionPaperId).FirstOrDefault();
            if (ifAssigned != null)
            {
                var a = _context.QuestionPaperIdentities.Where(x => x.QuestionPaperId == QuestionPaperId).FirstOrDefault();

                if (a != null)
                {
                    var questions = _context.QuestionPapers.Where(x => x.QuestionPaperId == a.QuestionPaperId).ToList();

                    if (questions.Count > 0)
                    {
                        ifAssigned.IsOpened = true;
                        ifAssigned.Inprogress = true;
                        ifAssigned.IsOpened = true;
                        ifAssigned.OpenedAt = DateTime.Now;
                        _context.TestStatuses.Update(ifAssigned);
                        _context.SaveChanges();
                        foreach (var items in questions)
                        {
                            TestQuestionsVM test = new TestQuestionsVM();
                            test.QuestionPaperId = a.QuestionPaperId;
                            test.QuestionId = items.QuestionId;
                            test.Question = items.Question;
                            test.OptionA = items.OptionA;
                            test.OptionB = items.OptionB;
                            test.OptionC = items.OptionC;
                            test.OptionD = items.OptionD;
                            test.QuestionType = items.QuestionType;
                            list.Add(test);
                        }
                    }
                    return list;
                }
            }
            return "Question Unavailable";
        }
        public string EditQuestionPaper(int QuestionPaperId, GenerateQustionPaper generate)
        {
            var Edit = _context.QuestionPaperIdentities.Where(x => x.QuestionPaperId == QuestionPaperId && x.IsDeleted != true).FirstOrDefault();
            if (Edit != null)
            {
                var DeletePre = _context.QuestionPapers.Where(x => x.QuestionPaperId == Edit.QuestionPaperId).ToList();
                if (DeletePre.Count > 0)
                {
                    foreach (var item in DeletePre)
                    {
                        _context.QuestionPapers.Remove(item);
                        _context.SaveChanges();
                    }
                }

                List<TestQuestionsVM> AllQuestions = new List<TestQuestionsVM>();
                List<TestQuestionsVM> SelectedQuestions = new List<TestQuestionsVM>();
                int totalmark = 0;
                dynamic a = null;

                if (generate.quetionMarkType == 0)
                {
                    a = _context.QuestionBanks.Where(x => x.SkillId.Equals(generate.SkillId) && x.SkillLevel.Equals(generate.Skilllevel) && x.QuestionLevel.Equals(generate.Questionlevel)).ToList();
                }
                else
                {
                    a = _context.QuestionBanks.Where(x => x.SkillId.Equals(generate.SkillId) && x.SkillLevel.Equals(generate.Skilllevel) && x.QuestionLevel.Equals(generate.Questionlevel) && x.Marks.Equals(generate.quetionMarkType)).ToList();
                }
                if (a.Count > 0)
                {
                    foreach (var x in a)
                    {
                        TestQuestionsVM test = new TestQuestionsVM();
                        test.QuestionId = x.QuestionId;
                        test.Question = x.Question;
                        test.OptionA = x.OptionA;
                        test.OptionB = x.OptionB;
                        test.OptionC = x.OptionC;
                        test.OptionD = x.OptionD;
                        test.Marks = x.Marks;
                        test.QuestionType = x.QuestionType;
                        AllQuestions.Add(test);
                    }
                    int count = AllQuestions.Count;
                    Random rand = new Random();
                    List<int> randomnumbers = new List<int>();
                    while (randomnumbers.Count < generate.NumberofQuestions)
                    {
                        randomnumbers.Add(rand.Next(count));
                    }
                    foreach (int index in randomnumbers)
                    {
                        SelectedQuestions.Add(AllQuestions[index]);
                        totalmark += (int)AllQuestions[index].Marks;
                    }
                }

                //QuestionPaperIdentity identity = new QuestionPaperIdentity();


                //identity.QuestionPaperName = generate.QuetionPaperName;
                //identity.SkillId = generate.SkillId;
                //identity.SkillLevel = generate.Skilllevel;
                //identity.QuestionLevel = generate.Questionlevel;
                //identity.NumberOfQuestions = generate.NumberofQuestions;
                //identity.MaximumMark = totalmark;
                //identity.QuestionMarkType = _context.QuestionMarkTypes.FirstOrDefault(c => c.QuestionMarkTypeId == generate.quetionMarkType).QuestionMarkTypeName;
                //identity.CreatedOn = DateTime.Now;
                //identity.IsDeleted = false;
                //_context.QuestionPaperIdentities.Add(identity);
                //_context.SaveChanges();
                //var Qid = identity.QuestionPaperId;

                foreach (var item in SelectedQuestions)
                {
                    QuestionPaper paper = new QuestionPaper();
                    paper.QuestionPaperId = QuestionPaperId;
                    paper.QuestionId = item.QuestionId;
                    paper.Question = item.Question;
                    paper.OptionA = item.OptionA;
                    paper.OptionB = item.OptionB;
                    paper.OptionC = item.OptionC;
                    paper.OptionD = item.OptionD;
                    paper.QuestionType = item.QuestionType;
                    _context.QuestionPapers.Add(paper);
                    _context.SaveChanges();
                }

                return "ok";

            }
            return "NotFound";
        }
        public List<TestQuestionsVM> Questions(int skillId, int qLevel)
        {
            List<TestQuestionsVM> list = new List<TestQuestionsVM>();
            List<TestQuestionsVM> selectedItems = new List<TestQuestionsVM>();
            var a = _context.QuestionBanks.Where(x => x.SkillId == skillId && x.QuestionLevel == qLevel).ToList();

            if (a.Count > 0)
            {
                foreach (var x in a)
                {
                    TestQuestionsVM test = new TestQuestionsVM();
                    test.QuestionId = x.QuestionId;
                    test.Question = x.Question;
                    test.OptionA = x.OptionA;
                    test.OptionB = x.OptionB;
                    test.OptionC = x.OptionC;
                    test.OptionD = x.OptionD;
                    list.Add(test);                    
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
        public string AssignToEmployee(string EmployeeIdentity, int QuestionpaperID)
        {
            var employee = _context.EmployeeModules.Where(x => x.EmployeeIdentity == EmployeeIdentity && x.IsDeleted != true && x.IsActivated == true).FirstOrDefault();
            if (employee != null)
            {
                var a = _context.QuestionPaperIdentities.Where(x => x.QuestionPaperId == QuestionpaperID && x.IsDeleted != true).FirstOrDefault();
                if (a != null)
                {
                    var b = _context.UserLevels.Where(x => x.EmployeeId == employee.EmployeeId && x.SkillId == a.SkillId).FirstOrDefault();
                    if (b != null)
                    {
                        var link = "https://localhost:7099/api/Training/GetQustionPaper?QuestionpaperID=" + a.QuestionPaperId;
                        var msg = "Hi" + employee.Name + " Click This Link and Attend The Test For Improve your Skill Level in " + a.SkillId + "<html><body><p>Click the link below:</p><a href=\"{link}\">{link}</a></body></html>";
                        var message = new Message(new string[] { employee.Email }, "Test", msg.ToString(), null, null);
                        var A = _emailservice.SendEmail(message);


                        if (A == "ok")
                        {
                            TestStatus status = new TestStatus();
                            status.EmployeeIdentity = EmployeeIdentity;
                            status.QuestionPaperId = QuestionpaperID;
                            status.IsAssigned = true;
                            status.IsOpened = false;
                            status.Inprogress = false;
                            status.IsCompleted = false;
                            status.IsValidated = false;
                            status.MaximumMark = a.MaximumMark;
                            status.MarksObtained = 0;
                            status.AssignedAt = DateTime.Now;
                            status.ItHaveADocument= false;
                            _context.TestStatuses.Add(status);
                            _context.SaveChanges();

                            var doc = _context.QuestionPapers.Where(x => x.QuestionPaperId == QuestionpaperID).ToList();

                            if (doc.Count > 0)
                            {
                                foreach (var items in doc)
                                {
                                    if (items.QuestionType == 3)
                                    {
                                        DocumentTypeAnswer document = new DocumentTypeAnswer();

                                        document.EmployeeIdentity = employee.EmployeeIdentity;
                                        document.SkillId = a.SkillId;
                                        document.QuestionPaperId = a.QuestionPaperId;
                                        document.QuestionNumber = items.QuestionId;
                                        document.AssigndAt = DateTime.Now;
                                        document.MaximumMark = _context.QuestionBanks.FirstOrDefault(x => x.QuestionId == items.QuestionId).Marks;
                                        document.IsNotified = false;
                                        document.IsValidated = false;
                                        document.IsUpdated = false;
                                        _context.DocumentTypeAnswers.Add(document);
                                        _context.SaveChanges();
                                        if (status.ItHaveADocument != true)
                                        {
                                            status.ItHaveADocument = true;
                                            _context.TestStatuses.Update(status);
                                            _context.SaveChanges();
                                        }
                                    }
                                }
                            }

                            return "Success";
                        }
                    }
                    else
                    {
                        return "NoSkill";
                    }
                }

                return "Questionpaper Not Found";
            }

            return "Employee Not Found";

        }
        public dynamic ValidateAnswers(List<ValidateVM> validate)
        {
            List<int> correctAnswers = new List<int>();
            List<int> WorngAnswers = new List<int>();
            List<int> SkippedQuestions = new List<int>();
            var Qid = 0;

            if (validate.Count > 0)
            {
                foreach (var item in validate)
                {
                    Qid = item.QuestionPaperId;
                    if (item.Answer == null)
                    {
                        SkippedQuestions.Add(item.QuestionId);
                        continue;
                    }
                    var a = _context.QuestionBanks.Where(x => x.QuestionId == item.QuestionId).FirstOrDefault();

                    if (a != null)
                    {
                        if (a.QuestionType == 1)
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
                        else if (a.QuestionType == 2)
                        {
                            string data = a.CorrectOption;
                            string[] values = data.Split(',');

                            string obj = item.Answer;
                            string[] det = obj.Split(",");

                            if (values.Count() == det.Count())
                            {
                                if (values.Count() == 1 && det.Count() == 1)
                                {
                                    var one = values[0];

                                    var oneans = det[0];

                                    if (one == oneans)
                                    {
                                        correctAnswers.Add(item.QuestionId);
                                    }
                                    else
                                    {
                                        WorngAnswers.Add(item.QuestionId);
                                    }

                                }
                                else if (values.Count() == 2 && det.Count() == 2)
                                {
                                    var one = values[0];
                                    var two = values[1];

                                    var oneans = det[0];
                                    var twoans = det[1];

                                    if ((one == oneans || one == twoans) && (two == oneans || two == twoans))
                                    {
                                        correctAnswers.Add(item.QuestionId);
                                    }
                                    else
                                    {
                                        WorngAnswers.Add(item.QuestionId);
                                    }
                                }
                                else if (values.Count() == 3 && det.Count() == 3)
                                {
                                    var one = values[0];
                                    var two = values[1];
                                    var three = values[2];

                                    var oneans = det[0];
                                    var twoans = det[1];
                                    var threeans = det[2];

                                    if ((one == oneans || one == twoans || one == threeans) && (two == oneans || two == twoans || two == threeans) && (three == oneans || three == twoans || three == threeans))
                                    {
                                        correctAnswers.Add(item.QuestionId);
                                    }
                                    else
                                    {
                                        WorngAnswers.Add(item.QuestionId);
                                    }
                                }
                                else
                                {
                                    var one = values[0];
                                    var two = values[1];
                                    var three = values[2];
                                    var four = values[3];

                                    var oneans = det[0];
                                    var twoans = det[1];
                                    var threeans = det[2];
                                    var fourans = det[3];

                                    if ((one == oneans || one == twoans || one == threeans || one == fourans) && (two == oneans || two == twoans || two == threeans || two == fourans) && (three == oneans || three == twoans || three == threeans || three == fourans) && (four == oneans || four == twoans || four == threeans || four == fourans))
                                    {
                                        correctAnswers.Add(item.QuestionId);
                                    }
                                    else
                                    {
                                        WorngAnswers.Add(item.QuestionId);
                                    }
                                }
                            }
                            //WorngAnswers.Add(item.QuestionId);

                           
                            
                        }
                        else if (a.QuestionType == 3)
                        {
                            if (item.Files != null)
                            {
                                var column = _context.DocumentTypeAnswers.Where(x => x.QuestionPaperId == item.QuestionPaperId && x.QuestionNumber == item.QuestionId).FirstOrDefault();
                                if (column != null)
                                {

                                    //IFormFile file = (IFormFile)item;

                                    //long length = file.Length;


                                    //using var fileStream = file.OpenReadStream();
                                    //byte[] bytes = new byte[length];
                                    //fileStream.Read(bytes, 0, (int)file.Length);

                                    string HTMLBody = "";

                                    using (StreamReader sReader = System.IO.File.OpenText(item.Files))
                                    {
                                        HTMLBody = sReader.ReadToEnd();
                                    }

                                    column.DocumentUrl = HTMLBody;
                                    column.SubmittedAt = DateTime.Now;
                                    _context.DocumentTypeAnswers.Update(column);
                                    _context.SaveChanges();
                                }
                            }
                            else
                            {
                                WorngAnswers.Add(item.QuestionId);
                            }
                        }
                    }
                }
                int Correct = correctAnswers.Count();
                int Wrong = WorngAnswers.Count();
                var s = _context.TestStatuses.Where(x => x.QuestionPaperId == Qid).FirstOrDefault();
                if (s != null)
                {
                    if(s.ItHaveADocument != true)
                    {
                        s.MarksObtained = Correct;
                        s.ValidatedAt= DateTime.Now;
                        
                        s.IsValidated= true;

                        _context.TestStatuses.Update(s);
                        _context.SaveChanges(); 
                    }
                }
                s.MarksObtained= Correct;
                s.IsCompleted = true;
                s.CompletedAt = DateTime.Now;
                _context.TestStatuses.Update(s);
                _context.SaveChanges();

                var result = new MyLists { Correct = correctAnswers, Wrong = WorngAnswers };
                return result;
            }
            return correctAnswers;
        }
        public dynamic GetFile(string fileContent)
        {
            string webRootPath = _hostingEnvironment.WebRootPath;
            string contentRootPath = _hostingEnvironment.ContentRootPath;
            string filePath = "";
            filePath = Path.Combine(contentRootPath, "Files", "file.txt");

            using (StreamWriter writer = new StreamWriter(filePath))
            {
                writer.Write(fileContent);
            }
            return filePath;
        }      
        public TestStatusDetails QuestionPaperStatus(int QuestionpaperId)
        {
            TestStatusDetails result = new TestStatusDetails();
            var data = _context.QuestionPaperIdentities.Where(x => x.QuestionPaperId == QuestionpaperId).FirstOrDefault();
            if (data != null)
            {
                var AssignedEmployees = _context.TestStatuses.Where(x => x.QuestionPaperId == QuestionpaperId && x.IsAssigned == true).Count();

                var NotOpend = _context.TestStatuses.Where(x => x.QuestionPaperId == QuestionpaperId && x.IsAssigned == true && x.IsOpened == false).Count();

                var Inprogress = _context.TestStatuses.Where(x => x.QuestionPaperId == QuestionpaperId && x.IsAssigned == true && x.IsOpened == true && x.Inprogress == true).Count();

                var Completed = _context.TestStatuses.Where(x => x.QuestionPaperId == QuestionpaperId && x.IsAssigned == true && x.IsOpened == true && x.IsCompleted == true).Count();

                var Validated = _context.TestStatuses.Where(x => x.QuestionPaperId == QuestionpaperId && x.IsAssigned == true && x.IsOpened == true && x.IsCompleted == true && x.IsValidated == true).Count();

                var PassedEmployee = _context.TestStatuses.Where(x => x.QuestionPaperId == QuestionpaperId && x.IsAssigned == true && x.IsOpened == true && x.IsCompleted == true && x.TestStatus1 == "Pass").Count();

                var FailedEmployee = _context.TestStatuses.Where(x => x.QuestionPaperId == QuestionpaperId && x.IsAssigned == true && x.IsOpened == true && x.IsCompleted == true && x.TestStatus1 == "Fail").Count();

                result.AssignedEmployees = AssignedEmployees;
                result.NotOpend = NotOpend;
                result.Inprogress = Inprogress;
                result.Completed = Completed;
                result.Validated = Validated;
                result.PassedEmployee = PassedEmployee;
                result.FailedEmployee = FailedEmployee;

            }
            return result;
        }
        public dynamic QuestionPaperStatusDetail(int QuestionpaperId)
        {
            List<EmployeeDetail> AssignedEmployee = new List<EmployeeDetail>();
            List<EmployeeDetail> OpenedQuestionPaper = new List<EmployeeDetail>();
            List<EmployeeDetail> TestInprogressEmployees = new List<EmployeeDetail>();
            List<EmployeeDetail> CompletedEmployees = new List<EmployeeDetail>();
            List<EmployeeDetail> PassedEmployees = new List<EmployeeDetail>();
            List<EmployeeDetail> FailedEmployees = new List<EmployeeDetail>();


            var AssignedEmployees = _context.TestStatuses.Where(x => x.QuestionPaperId == QuestionpaperId && x.IsAssigned == true).ToList();
            if (AssignedEmployee.Count > 0)
            {
                foreach (var emp in AssignedEmployee)
                {
                    var data = _context.EmployeeModules.Where(x => x.EmployeeIdentity == emp.EmployeeIdentity).FirstOrDefault();
                    EmployeeDetail detail = new EmployeeDetail();
                    detail.EmployeeIdentity = data.EmployeeIdentity;
                    detail.EmployeeName = data.Name;
                    detail.DepartmentId = (int)data.DepartmentId;
                    detail.DesignationId = (int)data.DesignationId;
                    AssignedEmployee.Add(detail);
                }
            }
            var NotOpend = _context.TestStatuses.Where(x => x.QuestionPaperId == QuestionpaperId && x.IsAssigned == true && x.IsOpened == false).ToList();
            if (NotOpend.Count > 0)
            {
                foreach (var emp in NotOpend)
                {
                    var data = _context.EmployeeModules.Where(x => x.EmployeeIdentity == emp.EmployeeIdentity).FirstOrDefault();
                    EmployeeDetail detail = new EmployeeDetail();
                    detail.EmployeeIdentity = data.EmployeeIdentity;
                    detail.EmployeeName = data.Name;
                    detail.DepartmentId = (int)data.DepartmentId;
                    detail.DesignationId = (int)data.DesignationId;
                    OpenedQuestionPaper.Add(detail);
                }
            }
            var Inprogress = _context.TestStatuses.Where(x => x.QuestionPaperId == QuestionpaperId && x.IsAssigned == true && x.IsOpened == true && x.Inprogress == true).ToList();
            if (Inprogress.Count > 0)
            {
                foreach (var emp in Inprogress)
                {
                    var data = _context.EmployeeModules.Where(x => x.EmployeeIdentity == emp.EmployeeIdentity).FirstOrDefault();
                    EmployeeDetail detail = new EmployeeDetail();
                    detail.EmployeeIdentity = data.EmployeeIdentity;
                    detail.EmployeeName = data.Name;
                    detail.DepartmentId = (int)data.DepartmentId;
                    detail.DesignationId = (int)data.DesignationId;
                    TestInprogressEmployees.Add(detail);
                }
            }
            var Completed = _context.TestStatuses.Where(x => x.QuestionPaperId == QuestionpaperId && x.IsAssigned == true && x.IsOpened == true && x.IsCompleted == true).ToList();
            if (Completed.Count > 0)
            {
                foreach (var emp in Completed)
                {
                    var data = _context.EmployeeModules.Where(x => x.EmployeeIdentity == emp.EmployeeIdentity).FirstOrDefault();
                    EmployeeDetail detail = new EmployeeDetail();
                    detail.EmployeeIdentity = data.EmployeeIdentity;
                    detail.EmployeeName = data.Name;
                    detail.DepartmentId = (int)data.DepartmentId;
                    detail.DesignationId = (int)data.DesignationId;
                    PassedEmployees.Add(detail);
                }
            }

            var PassedEmployee = _context.TestStatuses.Where(x => x.QuestionPaperId == QuestionpaperId && x.IsAssigned == true && x.IsOpened == true && x.IsCompleted == true && x.TestStatus1 == "Pass").ToList();
            if (PassedEmployee.Count > 0)
            {
                foreach (var emp in PassedEmployee)
                {
                    var data = _context.EmployeeModules.Where(x => x.EmployeeIdentity == emp.EmployeeIdentity).FirstOrDefault();
                    EmployeeDetail detail = new EmployeeDetail();
                    detail.EmployeeIdentity = data.EmployeeIdentity;
                    detail.EmployeeName = data.Name;
                    detail.DepartmentId = (int)data.DepartmentId;
                    detail.DesignationId = (int)data.DesignationId;
                    PassedEmployees.Add(detail);
                }
            }
            var FailedEmployee = _context.TestStatuses.Where(x => x.QuestionPaperId == QuestionpaperId && x.IsAssigned == true && x.IsOpened == true && x.IsCompleted == true && x.TestStatus1 == "Fail").ToList();
            if (FailedEmployee.Count > 0)
            {
                foreach (var emp in FailedEmployee)
                {
                    var data = _context.EmployeeModules.Where(x => x.EmployeeIdentity == emp.EmployeeIdentity).FirstOrDefault();
                    EmployeeDetail detail = new EmployeeDetail();
                    detail.EmployeeIdentity = data.EmployeeIdentity;
                    detail.EmployeeName = data.Name;
                    detail.DepartmentId = (int)data.DepartmentId;
                    detail.DesignationId = (int)data.DesignationId;
                    FailedEmployees.Add(detail);
                }
            }

            var result = new EmpList { Assigend = AssignedEmployee, NotOpened = OpenedQuestionPaper, InProgress = TestInprogressEmployees, Completed = CompletedEmployees, Passed = PassedEmployees, Failed = FailedEmployees };
            return result;
        }

        //Hangfire
        public void FindDocumented()
        {
            var result = _context.TestStatuses.Where(x => x.ItHaveADocument==true&&x.IsValidated!=true).FirstOrDefault();
            if (result != null)
            {
                DocumentNotification(result.EmployeeIdentity);
            }            
        }

        public void DocumentNotification(string Employeeidentity)
        {
            var job = _context.DocumentTypeAnswers.Where(x => x.EmployeeIdentity == Employeeidentity && x.SubmittedAt != null && x.IsNotified != true && x.IsValidated != true && x.DocumentUrl != null).ToList();
           
            byte[] fileBytes=new byte[0];
            List<IFormFile> files = new List<IFormFile>();
            List<filedata> byteFiles = new List<filedata>();
            if (job.Count > 0)
            {
                foreach(var answer in job)
                {                   
                   filedata filedata= new filedata();
                    if (!string.IsNullOrEmpty(answer.DocumentUrl))
                    {
                        string fileName = @"C:\Users\CIPL1246.COLANONLINE\source\repos\PMS\PMS_API\PMS_API\Files\" + answer.EmployeeIdentity + ".txt";
                        System.IO.File.WriteAllText(fileName, answer.DocumentUrl);
                        System.IO.FileStream fs = new System.IO.FileStream(fileName, System.IO.FileMode.Open, System.IO.FileAccess.Read);
                        System.IO.BinaryReader binaryReader = new System.IO.BinaryReader(fs);
                        long byteLength = new System.IO.FileInfo(fileName).Length;

                        filedata.ContentBytes = binaryReader.ReadBytes((Int32)byteLength);
                        filedata.FileName  = fileName;
                        fs.Close();
                        fs.Dispose();
                        binaryReader.Close();
                        byteFiles.Add(filedata);
                    }                    
                }               

                var data = _context.EmployeeModules.Where(x => x.EmployeeIdentity == Employeeidentity).FirstOrDefault();
                var managerdetails = _context.EmployeeModules.Where(y => y.EmployeeId == data.FirstLevelReportingManager).FirstOrDefault();
                var msg = "Hi" + managerdetails.Name + " " + data.Name + "(" + data.EmployeeIdentity + ")" + "Submitted Her Test Documents Please Verify and Evaluate the Document";

                var message = new Message(new string[] { managerdetails.Email }, "Test", msg.ToString(), null, byteFiles);

                var A = _emailservice.SendEmail(message);
                if(A == "ok")
                {
                    foreach(var item in job)
                    {
                        item.IsNotified= true;
                        _context.DocumentTypeAnswers.Update(item);
                        _context.SaveChanges();
                    }
                }
            }
        }

        public void documentValidation(List<DocumentValidationVM> document)
        {
            foreach(var item in document)
            {
                var Doc = _context.DocumentTypeAnswers.Where(x => x.DocumentId == item.documentid && x.IsValidated != true).FirstOrDefault();
                if (Doc != null)
                {
                    if (Doc.MaximumMark >= item.mark)
                    {
                        Doc.MarksObtained = item.mark;
                        Doc.IsValidated = true;
                        Doc.ValidatedAt = DateTime.Now;
                        _context.DocumentTypeAnswers.Update(Doc);
                        _context.SaveChanges();                       
                    }                    
                }
            }          

           
        }


        //Hangfire
        public void Updatemarks()
        {
         
            var ststus = _context.TestStatuses.Where(x => x.ItHaveADocument == true && x.IsValidated != true).ToList();
            if(ststus.Count > 0)
            {
                foreach(var items in ststus)
                {
                    var a = _context.DocumentTypeAnswers.Where(x => x.IsValidated== true&& x.IsUpdated!=true && x.QuestionPaperId == items.QuestionPaperId).ToList();
                    if (a.Count > 0)
                    {
                        foreach(var data in a)
                        {
                            items.MarksObtained = items.MarksObtained + data.MarksObtained;
                            _context.TestStatuses.Update(items);
                            _context.SaveChanges();
                            data.IsUpdated= true;
                            _context.DocumentTypeAnswers.Update(data);
                            _context.SaveChanges();

                        }
                        items.IsValidated = true;
                        items.ValidatedAt = DateTime.Now;
                        _context.TestStatuses.Update(items);
                        _context.SaveChanges();
                        TestStatusUpdate(items.StatusId);
                    }
              
                }
            }

        }


        public void TestStatusUpdate(int id)
        {
            var a = _context.TestStatuses.Where(y => y.StatusId == id && y.IsCompleted == true && y.IsValidated == true && y.TestStatus1 == null).FirstOrDefault();
            if(a!= null)
            {
                var getpercentage = (a.MarksObtained / a.MaximumMark) * 100;
                var passpercentage = _context.QuestionPaperIdentities.FirstOrDefault(x => x.QuestionPaperId == a.QuestionPaperId).PassPercentage;
                if(getpercentage >=passpercentage)
                {
                    a.TestStatus1 = "Pass";
                }
                else
                {
                    a.TestStatus1 = "Fail";
                }
                _context.TestStatuses.Update(a);
                _context.SaveChanges();
            }

        }

        public void ResultNotification()
        {
             var res = _context.TestStatuses.Where(x => x.ResulDeleiverd != true).FirstOrDefault();
            
            if(res!= null)
            {
                var skillid = _context.QuestionPaperIdentities.Where(x => x.QuestionPaperId == res.QuestionPaperId).FirstOrDefault();
                var skillname = _context.Skills.Where(x => x.SkillId == skillid.SkillId).FirstOrDefault();
                var emp = _context.EmployeeModules.Where(x => x.EmployeeIdentity == res.EmployeeIdentity).FirstOrDefault();
                if(emp!= null)
                {
                    var managerdetails = _context.EmployeeModules.Where(y => y.EmployeeId == emp.FirstLevelReportingManager).FirstOrDefault();
                    var msg = " Hi " + managerdetails.Name + " " + emp.Name + " (" + emp.EmployeeIdentity + ") " + " Completed Her Test for Update his Skill Level in " + skillname.SkillName + "<br>" + "TestMark = " + res.MarksObtained + " OutOff " + res.MaximumMark + "<br>" + "TestStatus = " + res.TestStatus1;
                    var message = new Message(new string[] { managerdetails.Email }, "Test", msg.ToString(), null, null);
                    var A = _emailservice.SendEmail(message);


                   
                    var Emsg = " Hi " + emp.Name + " (" + emp.EmployeeIdentity + ") " + " Your Test Document Evaluated Successfully You Scored "+  res.MarksObtained + " OutOff " + res.MaximumMark + "<br>" + "TestStatus = " + res.TestStatus1;
                    var Emessage = new Message(new string[] { managerdetails.Email }, "Test", Emsg.ToString(), null, null);
                    var E = _emailservice.SendEmail(Emessage);
                    if (E == "ok")
                    {
                        res.ResulDeleiverd = true;
                        _context.TestStatuses.Update(res);
                        _context.SaveChanges();
                    }
                }
            }
        }
    }
}
