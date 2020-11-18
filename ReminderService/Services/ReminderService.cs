using ReminderService.Exceptions;
using ReminderService.Models;
using ReminderService.Repository;
using System.Collections.Generic;
using System.Threading.Tasks;
namespace ReminderService.Services
{
    public class ReminderService : IReminderService
    {
        //Inherit the respective interface and implement the methods in 
        // this class i.e ReminderService by inheriting IReminderService
        private readonly IReminderRepository _reminderRepository;
        /*
      * ReminderRepository should  be injected through constructor injection. 
      * Please note that we should not create ReminderRepository object using the new keyword
      */
        public ReminderService(IReminderRepository reminderRepository)
        {
            _reminderRepository = reminderRepository;
        }

        public Task<bool> CreateReminder(string userId, string email, ReminderSchedule schedule)
        {
            var reminders = _reminderRepository.IsReminderExists(userId, schedule.NewsId).Result;
            if (reminders)
            {
                throw new ReminderAlreadyExistsException($"This News already have a reminder");
            }
            _reminderRepository.CreateReminder(userId, email, schedule);
            return Task.FromResult(true);
        }

        public Task<bool> DeleteReminder(string userId, int newsId)
        {
            var result = _reminderRepository.DeleteReminder(userId, newsId);
            if (!result.Result)
            {
                throw new NoReminderFoundException("No reminder found for this news");
            }
            return result;
        }

        public Task<List<ReminderSchedule>> GetReminders(string userId)
        {
            var result = _reminderRepository.GetReminders(userId);
            if (result.Result == null)
            {
                throw new NoReminderFoundException("No reminders found for this user");
            }
            return result;
        }

        public Task<bool> UpdateReminder(string userId, ReminderSchedule reminder)
        {
            var result = _reminderRepository.UpdateReminder(userId, reminder);
            if (!result.Result)
            {
                throw new NoReminderFoundException("No reminder found for this news");
            }
            return result;
        }

    }
}
