using CachePOC.ExternalModels;
using CachePOC.Models;
using CachePOC.Repositories;

namespace CachePOC.Controllers
{
    public class UnitController
    {
        private readonly UnitRepository _unitRepository = new UnitRepository();

        public Unit Get(long id)
        {
            return POCCacheAdapter.Instance.Get<Unit>(id);
        }

        public void Post(Unit unit)
        {
            _unitRepository.Insert(unit);
        }

        public void Put(Unit unit)
        {
            POCCacheAdapter.Instance.Add(unit, unit.Id);
        }

        public void Delete(Unit unit)
        {
            POCCacheAdapter.Instance.Remove<Unit>(unit.Id);
        }
    }
}
