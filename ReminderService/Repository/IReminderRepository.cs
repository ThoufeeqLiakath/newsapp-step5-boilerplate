using ReminderService.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
namespace ReminderService.Repository
{
    /*
	 * Should not modify this interface. You have to implement these methods of interface 
     * in corresponding Implementation classes
	 */
    public interface IReminderRepository
    {
        Task CreateReminder(string userId, string email, ReminderSchedule schedule);
        Task<bool> DeleteReminder(string userId, int newsId);
        Task<bool> UpdateReminder(string userId, ReminderSchedule reminder);
        Task<bool> IsReminderExists(string userId, int newsId);        
        Task<List<ReminderSchedule>> GetReminders(string userId);
    }
}
