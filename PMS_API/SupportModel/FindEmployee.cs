using PMS_API.Models;
using PMS_API.ViewModels;

namespace PMS_API.SupportModel
{
    public class FindEmployee
    {
        public int? Level { get; set; }
        public int? MinimumExperience { get; set; }
        public int? MaximumExperience { get; set; }
        public List<int> Skillid { get; set; }

    }


     
}
