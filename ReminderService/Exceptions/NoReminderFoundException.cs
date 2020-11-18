using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ReminderService.Exceptions
{
    public class NoReminderFoundException:ApplicationException
    {
        public NoReminderFoundException() { }
        public NoReminderFoundException(string message) : base(message) { }
    }
}
