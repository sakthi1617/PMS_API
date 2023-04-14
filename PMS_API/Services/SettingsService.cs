using Microsoft.EntityFrameworkCore;
using PMS_API.Data;
using PMS_API.Models;
using PMS_API.Repository;
using PMS_API.ViewModels;

namespace PMS_API.Services
{
    public class SettingsService:ISettingsRepo
    {
        private readonly PMSContext _context;
        private readonly IEmailService _emailService;

        public SettingsService(PMSContext context,IEmailService email)
        {
            _context = context;
            _emailService = email;       
            
        }
  
        public string TimeSetting(TimeSettingVM model)
        {
            var goalservice = new GoalService(_context, _emailService);
            var timesettingtbl = _context.TimeSettingTbls.FirstOrDefault(x => x.Id == 1);
            
            try
            {
                if (timesettingtbl != null)
                {
                    timesettingtbl.EmployeeReviewDay = model.EmployeeReviewDay;
                    timesettingtbl.ManagerReviewDay = model.ManagerReviewDay;
                    _context.TimeSettingTbls.Update(timesettingtbl);
                    _context.SaveChanges();                                      
                    goalservice.DateUpdate(model);
                    return "Date was successfully Updated";
                }
                else
                {
                    TimeSettingTbl ts = new TimeSettingTbl();
                    ts.EmployeeReviewDay = model.EmployeeReviewDay;
                    ts.ManagerReviewDay = model.ManagerReviewDay;
                    _context.TimeSettingTbls.Add(ts);
                    _context.SaveChanges();                    
                    goalservice.DateUpdate(model);
                    return "Date was Successfully Created";
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
