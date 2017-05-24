
namespace CachePOC.Models
{
    public class Manufacturer
    {
        public Manufacturer() { }

        public Manufacturer(long id, string name)
        {
            Id = id;
            Name = name;
        }

        public long Id { get; set; }
        public string Name { get; set; }
    }
}
