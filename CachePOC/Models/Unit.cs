using System.Collections.Generic;

namespace CachePOC.Models
{
    public class Unit
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public List<UnitProduct> Products { get; set; }
    }
}
