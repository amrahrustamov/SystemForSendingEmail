namespace Pustok.Database.Models;

public class Product
{
    public Product(string name, string decription, string color, string size, decimal price, int? categoryId, string physicalImageName)
    {
        Name = name;
        Description = decription;
        Color = color;
        Size = size;
        Price = price;
        //CategoryId = categoryId;
        PhysicalImageName = physicalImageName;
    }

    public Product() { }

    public int Id { get; set; }

    public string Name { get; set; }
    public string Description { get; set; }
    public string Color { get; set; }
    public string Size { get; set; }
    public decimal Price { get; set; }
    public string PhysicalImageName { get; set; }

    public List<CategoryProduct> CategoryProducts { get; set; }
}
