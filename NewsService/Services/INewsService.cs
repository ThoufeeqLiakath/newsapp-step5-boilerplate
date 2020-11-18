using NewsService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NewsService.Services
{
    /*
     * Should not modify this interface. You have to implement these methods of interface 
     * in corresponding Implementation classes
   */
    public interface INewsService
    {
        Task<int> CreateNews(string userId, News news);
        Task<bool> DeleteNews(string userId, int newsId);
        Task<List<News>> FindAllNewsByUserId(string userId);
        Task<bool> AddOrUpdateReminder(string userId, int newsId, Reminder reminder);
        Task<bool> DeleteReminder(string userId, int newsId);
    }
}
