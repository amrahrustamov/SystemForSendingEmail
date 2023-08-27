using Pustok.Database.Models;

namespace Pustok.ViewModels.Client
{
    public class HomeViewModel
    {
        public List<Product> Products { get; set; }
        public List<SlideBanner> SlideBanners { get; set; }
        public List<Email> EmailAddresses { get; set; }
    }
}
