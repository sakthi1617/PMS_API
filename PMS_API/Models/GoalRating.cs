using System;
using System.Collections.Generic;

namespace PMS_API.Models
{
    public partial class GoalRating
    {
        public int RatingId { get; set; }
        public int? EmployeeId { get; set; }
        public int? ManagerId { get; set; }
        public decimal? RatingbyEmployee { get; set; }
        public DateTime? RatingbyEmployeeCalculatedAt { get; set; }
        public decimal? RatingbyManager { get; set; }
        public DateTime? RatingbyManagerCalculatedAt { get; set; }
        public virtual EmployeeModule? Employee { get; set; }
    }
}
