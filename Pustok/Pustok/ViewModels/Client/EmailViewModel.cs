using Pustok.Database.Models;

namespace Pustok.ViewModels.Client
{
    public class EmailViewModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public string EmailAddress { get; set; }
        public DateTime SendingTime { get; set; }

    }
}
