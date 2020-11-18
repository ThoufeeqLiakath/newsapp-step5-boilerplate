using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ReminderService.Exceptions
{
    public class ReminderAlreadyExistsException:ApplicationException
    {
        public ReminderAlreadyExistsException() { }
        public ReminderAlreadyExistsException(string message) : base(message) { }
    }
}
