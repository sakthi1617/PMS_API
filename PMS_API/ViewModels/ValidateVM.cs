﻿namespace PMS_API.ViewModels
{
    public class ValidateVM
    {
        public int QuestionId { get; set; } 

        public string Answer { get; set; }


    }
    public class MyLists
    {
        public List<int>? Correct { get; set; }
        public List<int>? Wrong { get; set; }
    }

}
