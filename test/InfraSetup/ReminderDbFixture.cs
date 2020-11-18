using Microsoft.Extensions.Configuration;
using MongoDB.Driver;
using ReminderService.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Test.InfraSetup
{
    public class ReminderDbFixture:IDisposable
    {
        private IConfigurationRoot configuration;
        public ReminderContext context;
        public ReminderDbFixture()
        {
            var builder = new ConfigurationBuilder().AddJsonFile("appsettings.json");
            configuration = builder.Build();
            context = new ReminderContext(configuration);
            context.Reminders.DeleteMany(Builders<Reminder>.Filter.Empty);
            context.Reminders.InsertMany(new List<Reminder>
            {
                new Reminder{ 
                    UserId="Jack",
                    Email="jack@ymail.com",
                    NewsReminders=new List<ReminderSchedule>{
                        new ReminderSchedule { NewsId=101, Schedule=DateTime.Now } 
                    } }
            }); 
        }
        public void Dispose()
        {
            context.Reminders.DeleteMany(Builders<Reminder>.Filter.Empty);
            context = null;
        }
    }
}
