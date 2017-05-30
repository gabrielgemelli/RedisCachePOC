using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CachePOC.Models
{
    public class Diet
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public List<DietUnit> Units { get; set; }
        public List<DietProduct> Products { get; set; }
    }
}
