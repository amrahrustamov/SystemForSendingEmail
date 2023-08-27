using System.ComponentModel.DataAnnotations;

namespace Pustok.ViewModels.Admin.Product;

public class ProductAddViewModel : BaseProductViewModel
{
    [Required]
    public int[] CategoryIds { get; set; }
    public List<Database.Models.Category> Categories { get; set; }

    public IFormFile Image { get; set; }
}
