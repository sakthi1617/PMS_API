﻿using PMS_API.Models;

namespace PMS_API.ViewModels
{
    public class QuestionBankVM
    {
        public int? SkillId { get; set; }
        public string? Question { get; set; }
        public string? OptionA { get; set; }
        public string? OptionB { get; set; }
        public string? OptionC { get; set; }
        public string? OptionD { get; set; }
        public string? CorrectOption { get; set; }
        public int? QuestionLevel { get; set; }
        public int? QuestionType { get; set; }

        public int? SkillLevel { get; set; }
        public int? Marks { get; set; }

    }
}


