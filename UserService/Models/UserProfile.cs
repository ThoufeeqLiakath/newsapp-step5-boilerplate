using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
namespace UserService.Models
{
    public class UserProfile
    {
        /*
      * This class should have a property called UserId which returns integer data type
      * the field userId should be annotated with [BsonId]
      * FirstName property returns a string data type
      * LastName property returns a string data type
      * Contact property returns a string data type
      * Email property returns a string data type
      * CreatedAt property returns a DateTime data type
      */
        [BsonId]
        public string UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Contact{ get; set; }
        public string Email { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
