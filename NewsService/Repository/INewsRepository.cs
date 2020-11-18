using NewsService.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NewsService.Repository
{
    /*
	 * Should not modify this interface. You have to implement these methods of interface 
     * in corresponding Implementation classes
	 */
    public interface INewsRepository
    {
        Task<bool> AddOrUpdateReminder(string userId, int newsId, Reminder reminder);
        Task<int> CreateNews(string userId, News news);
        Task<bool> DeleteNews(string userId, int newsId);
        Task<News> GetNewsById(string userId, int newsId);
        Task<bool> DeleteReminder(string userId, int newsId);
        Task<List<News>> FindAllNewsByUserId(string userId);
        Task<bool> IsNewsExist(string userId, string title);
        Task<bool> IsReminderExists(string userId, int newsId);
    }
}