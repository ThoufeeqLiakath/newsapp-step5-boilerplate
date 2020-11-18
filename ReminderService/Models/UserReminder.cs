using MongoDB.Bson.Serialization.Attributes;
using System.Collections.Generic;
namespace ReminderService.Models
{
    public class Reminder
    {
        /*
      * This class should have a property called UserId which returns string data type and 
      * the field userId should be annotated with [BsonId],
      * Email Property which returns string data type
      * and NewsReminders List which returns List of ReminderSchedule class type i.e List<ReminderSchedule>
      */
        [BsonId]
        public string UserId { get; set; }        
        public string Email { get; set; }
        public List<ReminderSchedule> NewsReminders { get; set; }

    }
}
