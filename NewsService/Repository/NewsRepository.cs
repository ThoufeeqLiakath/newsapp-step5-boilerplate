using MongoDB.Driver;
using NewsService.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
namespace NewsService.Repository
{
    //Inherit the respective interface and implement the methods in
    // this class i.e NewsRepository by inheriting INewsRepository
    public class NewsRepository : INewsRepository
    {
        //define a private variable to represent NewsDbContext

        private readonly NewsContext _newsContext;
        public NewsRepository(NewsContext newsContext)
        {
            _newsContext = newsContext;
        }

        public Task<bool> AddOrUpdateReminder(string userId, int newsId, Reminder reminder)
        {
            var userNewws = _newsContext.News.Find(x => x.UserId == userId).FirstOrDefault();
            var newsobj = new News() { Reminder = reminder, NewsId = newsId };
            var filter = Builders<UserNews>.Filter.And(
                                    Builders<UserNews>.Filter.Where(x => x.UserId == userId),
                                    Builders<UserNews>.Filter.ElemMatch(x => x.NewsList, c => c.NewsId == newsId));
            if (userNewws == null)
            {
                var newUserNews = new UserNews()
                {
                    UserId = userId,
                    NewsList = new List<News>() { newsobj }
                };
                _newsContext.News.InsertOne(newUserNews);
            }
            else
            {
                var news = userNewws.NewsList.Where(x => x.NewsId == newsId).FirstOrDefault();
                

                UpdateResult update;
                if (news == null)
                {
                    //userNewws.NewsList = new List<News>() { newsobj };
                    update = _newsContext.News.UpdateOne(filter, Builders<UserNews>.Update.AddToSet(x => x.NewsList, newsobj));
                }
                else
                {
                    update = _newsContext.News.UpdateOne(filter, Builders<UserNews>.Update.Set(x => x.NewsList[-1].Reminder, reminder)
                                                                                                    .Set(x => x.NewsList[-1].NewsId, newsId));
                }
                var getUpdatedCount = update.ModifiedCount;                
            }
            var updatedValue = _newsContext.News.Find(filter).FirstOrDefault();
            var result = updatedValue != null;
            return Task.FromResult(result);           

        }

        public Task<int> CreateNews(string userId, News news)
        {
            var userNews = _newsContext.News.Find(x => x.UserId == userId).FirstOrDefault();

            var maxNewsId = userNews == null ? 100 : userNews.NewsList.Max(x => x.NewsId);

            if (news.NewsId == 0)
            {
                news.NewsId = maxNewsId + 1;
            }
            if (userNews == null)
            {
                userNews = new UserNews()
                {
                    UserId = userId,
                    NewsList = new List<News>() { news }
                };
                _newsContext.News.InsertOne(userNews);
            }
            else
            {
                var filter = Builders<UserNews>.Filter.Where(x => x.UserId == userId);
                var update = Builders<UserNews>.Update.Push(x => x.NewsList, news);
                var created = _newsContext.News.FindOneAndUpdate(filter, update);
            }

            var createdNewsId = _newsContext.News.Find(x => x.UserId == userId).FirstOrDefault().NewsList.Where(x => x.NewsId == news.NewsId).FirstOrDefault().NewsId;
            return Task.FromResult(createdNewsId);
        }

        public Task<bool> DeleteNews(string userId, int newsId)
        {
            var filter = Builders<UserNews>.Filter.And(
                Builders<UserNews>.Filter.Where(x => x.UserId == userId)               
                );
            var news = _newsContext.News.Find(x => x.UserId == userId).FirstOrDefault();
            if (news == null)
            {
                return Task.FromResult(false);
            }
            var news1 = news.NewsList.Where(x => x.NewsId == newsId).FirstOrDefault();
            if (news1 == null)
            {
                return Task.FromResult(false);
            }
            var update = _newsContext.News.FindOneAndUpdate(filter, Builders<UserNews>.Update.PullFilter(x => x.NewsList, c => c.NewsId == newsId));
            //var result = _newsContext.UserNews.DeleteOne(filter);
            var deleted = _newsContext.News.Find(x => x.UserId == userId).FirstOrDefault().NewsList.Where(x=>x.NewsId==newsId).FirstOrDefault();
            return Task.FromResult(deleted==null);
        }

        public Task<bool> DeleteReminder(string userId, int newsId)
        {
            var news = _newsContext.News.Find(x => x.UserId == userId).FirstOrDefault();
            if (news == null || news.NewsList == null || news.NewsList.Count == 0)
            {
                return Task.FromResult(false);
            }
            var reminder = news.NewsList.Where(x => x.NewsId == newsId).FirstOrDefault();
            if (reminder == null)
            {
                return Task.FromResult(false);
            }
            var list = news.NewsList.Where(x => x.NewsId != newsId);
            var filter = Builders<UserNews>.Filter.And(
                Builders<UserNews>.Filter.Where(x => x.UserId == userId),
                Builders<UserNews>.Filter.ElemMatch(x => x.NewsList, c => c.NewsId == newsId)
                );
            var update = Builders<UserNews>.Update.Set(x => x.NewsList, list);
            var r = _newsContext.News.FindOneAndUpdate(filter, update);
            var rr = _newsContext.News.Find(filter).FirstOrDefault();
            return Task.FromResult(rr == null);

        }

        public Task<List<News>> FindAllNewsByUserId(string userId)
        {
            List<News> newsLIst = null;
            //var projection = Builders<UserNews>.Projection.Expression(x => x.NewsList);
            var result = _newsContext.News.Find(x => x.UserId == userId).FirstOrDefault();//.Project(projection).FirstOrDefault();
            if (result == null)
            {
                return Task.FromResult(newsLIst);
            }
            return Task.FromResult(result.NewsList);
        }

        public Task<News> GetNewsById(string userId, int newsId)
        {
            var projection = Builders<UserNews>.Projection.Expression(x => x.NewsList.Where(y => y.NewsId == newsId).FirstOrDefault());
            return Task.FromResult(_newsContext.News.Find(x => x.UserId == userId).Project(projection).FirstOrDefault());
        }

        public Task<bool> IsNewsExist(string userId, string title)
        {
            var filter = Builders<UserNews>.Filter.And
                (
                Builders<UserNews>.Filter.Where(x => x.UserId == userId),
                Builders<UserNews>.Filter.ElemMatch(x => x.NewsList, c => c.Title == title)
                );
            var result = _newsContext.News.Find(filter).AnyAsync();
            return result;
        }

        public Task<bool> IsReminderExists(string userId, int newsId)
        {
            var filter = Builders<UserNews>.Filter.And
                (
                Builders<UserNews>.Filter.Where(x => x.UserId == userId)
                //,
                //Builders<UserNews>.Filter.ElemMatch(x => x.NewsList, c => c.NewsId == newsId)
                );
            if (!_newsContext.News.Find(filter).Any())
            {
                return Task.FromResult(false);
            }
            //var projection = Builders<UserNews>.Projection.Expression(x => x.NewsList.Where(n => n.NewsId == newsId).FirstOrDefault());
            var t = _newsContext.News.Find(filter).FirstOrDefault().NewsList.Where(x => x.NewsId == newsId).FirstOrDefault();
            if (t == null)
            {
                return Task.FromResult(false);
            }
            var result = t.Reminder != null;
            return Task.FromResult(result);
        }

    }
}
