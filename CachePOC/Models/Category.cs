
namespace CachePOC.Models
{
    public class Category
    {
        public Category() { }

        public Category(long id, string name)
        {
            Id = id;
            Name = name;
        }

        public long Id { get; set; }
        public string Name { get; set; }
    }
}
