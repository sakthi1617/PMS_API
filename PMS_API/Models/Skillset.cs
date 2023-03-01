using System;
using System.Collections.Generic;

namespace PMS_API.Models
{
    public partial class Skillset
    {
        public int Id { get; set; }
        public int? DesignationId { get; set; }
        public string? Skills { get; set; }

        public virtual Department? Designation { get; set; }
    }
}
