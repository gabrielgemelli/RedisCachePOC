
namespace CachePOC.Models
{
    public class Product
    {
        public Product() { }

        public Product(long id, string name, Category category, Manufacturer manufacturer) {
            Id = id;
            Name = name;
            Category = category;
            Manufacturer = manufacturer;
        }

        public long Id { get; set; }
        public string Name { get; set; }
        public Category Category { get; set; }
        public Manufacturer Manufacturer { get; set; }
    }
}
