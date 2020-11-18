using ReminderService.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ReminderService.Services
{
    /*
	 * Should not modify this interface. You have to implement these methods of interface 
     * in corresponding Implementation classes
	 */
    public interface IReminderService
    {
        Task<bool> CreateReminder(string userId, string email, ReminderSchedule schedule);
        Task<bool> DeleteReminder(string userId, int newsId);
        Task<bool> UpdateReminder(string userId, ReminderSchedule reminder);
        Task<List<ReminderSchedule>> GetReminders(string userId);
    }
}
