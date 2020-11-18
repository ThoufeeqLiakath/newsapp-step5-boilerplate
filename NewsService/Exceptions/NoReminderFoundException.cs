using System;

namespace NewsService.Exceptions
{
    public class NoReminderFoundException:ApplicationException
    {
        public NoReminderFoundException() { }
        public NoReminderFoundException(string message) : base(message) { }
    }
}
