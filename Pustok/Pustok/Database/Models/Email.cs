using Microsoft.EntityFrameworkCore;

namespace Pustok.Database.Models
{
    public class Email
    {
        public Email()
        {
            // Initialize default values or leave properties empty
        }
        public Email(string title, string content, string emailAddress, DateTime sendingTime)
        {
            Title = title;
            Content = content;
            EmailAddress = emailAddress;
            SendingTime = sendingTime;
        }

        public int Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public string EmailAddress { get; set; }
        public DateTime SendingTime { get; set; }

    }
}
