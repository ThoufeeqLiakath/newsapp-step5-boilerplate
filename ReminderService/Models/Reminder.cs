using MongoDB.Bson.Serialization.Attributes;
using System;
namespace ReminderService.Models
{
    public class ReminderSchedule
    {
        /*
       * This class should have a property called NewsId which returns integer data type
       * and Schedule property which returns DateTime data type
       */
        public int NewsId { get; set; }
        public DateTime Schedule { get; set; }

    }
}
