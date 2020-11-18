using MongoDB.Bson.Serialization.Attributes;
using System.Collections.Generic;

namespace NewsService.Models
{
    public class UserNews
    {
        /*
         * This class should have two properties (UserId, NewsList).
         * Out of these two fields, the field userId should be annotated with [BsonId].
         * NewsList property returns a list of News
         */
        [BsonId]
        public string UserId { get; set; }
        public List<News> NewsList { get; set; }

    }
}
