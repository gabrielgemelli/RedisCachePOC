using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CachePOC.ExternalModels;
using CachePOC.Models;

namespace CachePOC.Repositories
{
    public class UnitRepository
    {
        private readonly UnitProductRepository _unitProductRepository = new UnitProductRepository();

        public void Insert(Unit unit)
        {
            POCCacheAdapter.Instance.Add(unit, unit.Id);

            unit.Products?.ForEach(_unitProductRepository.Insert);
        }
    }
}
