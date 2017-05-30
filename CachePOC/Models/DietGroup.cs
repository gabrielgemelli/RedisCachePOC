using System.Collections.Generic;

namespace CachePOC.Models
{
    public class DietGroup
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public List<Diet> Diets { get; set; }
    }
}
