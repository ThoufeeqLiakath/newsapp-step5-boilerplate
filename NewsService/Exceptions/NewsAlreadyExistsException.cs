using System;

namespace NewsService.Exceptions
{
    public class NewsAlreadyExistsException:ApplicationException
    {
        public NewsAlreadyExistsException() { }
        public NewsAlreadyExistsException(string message) : base(message) { }
    }
}
