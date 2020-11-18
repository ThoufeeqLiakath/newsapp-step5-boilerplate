using MongoDB.Driver;
using ReminderService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
namespace ReminderService.Repository
{
    //Inherit the respective interface and implement the methods in 
    // this class i.e ReminderRepository by inheriting IReminderRepository class 
    //which is used to implement all Data access operations
    public class ReminderRepository : IReminderRepository
    {
        //define a private variable to represent Reminder Database Context
        private readonly ReminderContext _reminderContext;

        public ReminderRepository(ReminderContext reminderContext)
        {
            _reminderContext = reminderContext;
        }

        public Task CreateReminder(string userId, string email, ReminderSchedule schedule)
        {
            var list = _reminderContext.Reminders.Find(x => x.UserId == userId).FirstOrDefault();
            var maxNewsId = list == null ? 100 : list.NewsReminders.Max(x => x.NewsId);
            var filter = Builders<Reminder>.Filter.And(
                Builders<Reminder>.Filter.Where(x => x.UserId == userId && x.Email == email)
            );
            if (schedule.NewsId == 0)
            {
                schedule.NewsId = maxNewsId;
            }
            if (list == null)
            {
                var reminder = new Reminder()
                {
                    UserId = userId,
                    Email = email,
                    NewsReminders = new List<ReminderSchedule>()
                    {
                       schedule
                    }
                };
                _reminderContext.Reminders.InsertOne(reminder);
            }
            else
            {
                list.NewsReminders.Add(schedule);
                var reminder = new Reminder()
                {
                    UserId = userId,
                    Email = email,
                    NewsReminders = list.NewsReminders
                };
                var update = Builders<Reminder>.Update.Set(x => x.NewsReminders, list.NewsReminders);
                _reminderContext.Reminders.UpdateOne(filter, update);
            }
            var createdReminder = _reminderContext.Reminders.Find(filter).FirstOrDefault();
            return Task.FromResult(createdReminder);
        }

        public Task<bool> DeleteReminder(string userId, int newsId)
        {            
            var result = _reminderContext.Reminders.Find(x=>x.UserId==userId).FirstOrDefault();
            
            if (result == null)
            {
                return Task.FromResult(false);
            }
            var val = result.NewsReminders.Where(x => x.NewsId == newsId).FirstOrDefault();
            if(val==null)
            {
                return Task.FromResult(false);
            }
            var list = result.NewsReminders.Where(x=>x.NewsId!=newsId).ToList();            
            result.NewsReminders = list;
            var update = Builders<Reminder>.Update.Set(x => x.NewsReminders, list);
            var filter = Builders<Reminder>.Filter.Where(x => x.UserId == userId);
            var deleted = _reminderContext.Reminders.UpdateOne(filter,update);
            var result1 = _reminderContext.Reminders.Find(x => x.UserId == userId).FirstOrDefault().NewsReminders.Where(x=>x.NewsId==newsId).FirstOrDefault();
            return Task.FromResult(result1 == null);
        }

        public Task<List<ReminderSchedule>> GetReminders(string userId)
        {
            List<ReminderSchedule> tets = null;
            var reminders = _reminderContext.Reminders.Find(x => x.UserId == userId).FirstOrDefault();
            if (reminders == null)
            {
                return Task.FromResult(tets);
            }
            return Task.FromResult(reminders.NewsReminders);
        }

        public Task<bool> IsReminderExists(string userId, int newsId)
        {
            var userExist = _reminderContext.Reminders.Find(x => x.UserId == userId).FirstOrDefault();
            if (userExist == null)
            {
                return Task.FromResult(false);
            }
            var reminderExist = userExist.NewsReminders.Where(x => x.NewsId == newsId).FirstOrDefault();
            return Task.FromResult(reminderExist != null);
        }

        public Task<bool> UpdateReminder(string userId, ReminderSchedule reminder)
        {
            var reminderFromDb = _reminderContext.Reminders.Find(x => x.UserId == userId).FirstOrDefault();
            if (reminderFromDb == null)
            {
                return Task.FromResult(false);
            }
            var reminderList = reminderFromDb.NewsReminders;
            if (reminderList == null || reminderList.Count == 0)
            {
                return Task.FromResult(false);
            }
            var reminderUserExists = reminderList.Where(x => x.NewsId == reminder.NewsId).FirstOrDefault();
            if (reminderUserExists == null)
            {
                return Task.FromResult(false);
            }
            var filter1 = Builders<Reminder>.Filter.And(
                Builders<Reminder>.Filter.Where(x => x.UserId == userId),
                Builders<Reminder>.Filter.ElemMatch(x => x.NewsReminders, c => c.NewsId == reminder.NewsId)
                );
            var e = _reminderContext.Reminders.Find(x => x.UserId == userId).FirstOrDefault();
            var eresult = _reminderContext.Reminders.Find(filter1).FirstOrDefault();
            if (eresult == null)
            {
                return Task.FromResult(false);
            }
            var update = Builders<Reminder>.Update.Set(x => x.NewsReminders[-1], reminder);
            _reminderContext.Reminders.FindOneAndUpdate(filter1, update);
            var result = _reminderContext.Reminders.Find(filter1).FirstOrDefault();
            var m = result.NewsReminders.Where(x => x.NewsId == reminder.NewsId).FirstOrDefault();
            var incomingReminderDate = reminder.Schedule.ToString("MM/dd/yyyy");
            var updatedReminderDate = m.Schedule.ToString("MM/dd/yyyy");
            return Task.FromResult(incomingReminderDate == updatedReminderDate);
        }
    }
}
