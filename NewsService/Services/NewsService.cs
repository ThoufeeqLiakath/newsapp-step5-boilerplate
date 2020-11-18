using NewsService.Models;
using NewsService.Repository;
using NewsService.Exceptions;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;

namespace NewsService.Services
{
    //Inherit the respective interface and implement the methods in 
    // this class i.e NewsService by inheriting INewsService

    public class NewsService : INewsService
    {
        private readonly INewsRepository _newsRepository;
        public NewsService(INewsRepository newsRepository)
        {
            _newsRepository = newsRepository;
        }
        public Task<bool> AddOrUpdateReminder(string userId, int newsId, Reminder reminder)
        {
            var reminderExists = _newsRepository.GetNewsById(userId,newsId).Result;
            if (reminderExists==null)
            {
                throw new NoNewsFoundException($"NewsId {newsId} for {userId} doesn't exist");
            }
            return _newsRepository.AddOrUpdateReminder(userId, newsId, reminder);
        }

        public Task<int> CreateNews(string userId, News news)
        {
            var result= _newsRepository.IsNewsExist(userId,news.Title).Result;
            if (result)
            {
                throw new NewsAlreadyExistsException($"{userId} have already added this news");
            }
            return _newsRepository.CreateNews(userId, news);
        }

        public Task<bool> DeleteNews(string userId, int newsId)
        {
            var result = _newsRepository.DeleteNews(userId, newsId);
            if (!result.Result)
            {
                throw new NoNewsFoundException($"NewsId {newsId} for {userId} doesn't exist");
            }
            return result;
        }

        public Task<bool> DeleteReminder(string userId, int newsId)
        {
            var reminderExists = _newsRepository.IsReminderExists(userId, newsId).Result;
            if (!reminderExists)
            {
                throw new NoReminderFoundException("No reminder found for this news");
            }
            return _newsRepository.DeleteReminder(userId, newsId);
        }

        public Task<List<News>> FindAllNewsByUserId(string userId)
        {
            var result = _newsRepository.FindAllNewsByUserId(userId);
            if (result.Result == null)
            {
                throw new NoNewsFoundException($"No news found for {userId}");
            }
            return result;
        }
    }
}
