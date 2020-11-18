using System;

namespace NewsService.Exceptions
{
    public class NoNewsFoundException:ApplicationException
    {
        public NoNewsFoundException() { }
        public NoNewsFoundException(string message) : base(message) { }
    }
}
