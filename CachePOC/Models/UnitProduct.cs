using CachePOC.ExternalModels;

namespace CachePOC.Models
{
    public class UnitProduct
    {
        public long Id { get; set; }
        public long UnitId { get; set; }
        public Product Product { get; set; }
    }
}
