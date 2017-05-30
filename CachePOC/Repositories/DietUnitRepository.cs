using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CachePOC.Models;

namespace CachePOC.Repositories
{
    public class DietUnitRepository
    {
        public void Insert(DietUnit dietUnit)
        {
            AddToCache(dietUnit);
        }

        public void Update(DietUnit dietUnit)
        {
            AddToCache(dietUnit);
        }

        private static void AddToCache(DietUnit dietUnit)
        {
            POCCacheAdapter.Instance.Add(dietUnit, dietUnit.Id);
            POCCacheAdapter.Instance.Add(dietUnit.Unit, dietUnit.UnitId);

            POCCacheAdapter.Instance.SetDependency<Diet, DietUnit>(dietUnit.DietId, dietUnit.Id);

            POCCacheAdapter.Instance.SetDependency<DietUnit, Diet>(dietUnit.Id, dietUnit.DietId);
            POCCacheAdapter.Instance.SetDependency<Unit, DietUnit>(dietUnit.UnitId, dietUnit.Id);
        }
    }
}
