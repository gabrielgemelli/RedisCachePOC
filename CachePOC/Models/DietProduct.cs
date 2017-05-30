using CachePOC.ExternalModels;

namespace CachePOC.Models
{
    public class DietProduct
    {
        public long Id { get; set; }
        public long DietId { get; set; }
        public long ProductId { get; set; }
        public Product Product { get; set; }
    }
}