namespace Pustok.Database.Models;

public class Category : object
{
    public int Id { get; set; }
    public string Name { get; set; }


    public override string ToString()
    {
        return $"Id : {Id}, Name : {Name}";
    }

    public List<CategoryProduct> CategoryProducts { get; set; }
}
