using System;

namespace NewsService.Models
{
    public class News
    {
        /*
    * This class should have seven properies
    * (NewsId,Title,Content,PublishedAt, Url,UrlToImage and Reminder).
    * NewsId returns int data type
    * Title returns string data type
    * Cotent returns string data type
    * PublishedAt returns DateTime data type
    * Url returns string data type
    * UrlToImage returns string data type
    * Reminder property returns as Reminder class
    */

        public int NewsId { get; set; }
        public string Title{ get; set; }
        public string Content { get; set; }
        public DateTime PublishedAt { get; set; }
        public string Url { get; set; }
        public string UrlToImage { get; set; }
        public Reminder Reminder{ get; set; }

    }
}
