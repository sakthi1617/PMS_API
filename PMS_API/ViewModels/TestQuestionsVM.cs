﻿namespace PMS_API.ViewModels
{
    public class TestQuestionsVM
    {
        public int? QuestionPaperId { get; set; }   
        public int? QuestionId { get; set; } 
        public string? Question { get; set; }
        public string? OptionA { get; set; }
        public string? OptionB { get; set; }
        public string? OptionC { get; set; }
        public string? OptionD { get; set; }
        public int? QuestionType { get; set; }
        public int? Marks { get; set;}
    }
}
