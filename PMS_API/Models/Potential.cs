using System;
using System.Collections.Generic;

namespace PMS_API.Models
{
    public partial class Potential
    {
        public int PotentialId { get; set; }
        public int? PotentialLevel { get; set; }
        public string? PotentialName { get; set; }
    }
}
