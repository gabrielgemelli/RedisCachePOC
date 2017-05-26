using CachePOC.ExternalModels;
using CachePOC.Models;

namespace CachePOC.Controllers
{
    public class UnitController
    {
        public Unit Get(long id)
        {
            return POCCacheAdapter.Instance.Get<Unit>(id);
        }

        public void Post(Unit unit)
        {
            POCCacheAdapter.Instance.Add(unit, unit.Id);
            unit.Products?.ForEach(unitProduct =>
            {
                POCCacheAdapter.Instance.Add(unitProduct, unitProduct.Id);
                POCCacheAdapter.Instance.Add(unitProduct.Product, unitProduct.Product.Id);

                POCCacheAdapter.Instance.SetDependency<Unit, UnitProduct>(unit.Id, unitProduct.Id);

                POCCacheAdapter.Instance.SetDependency<UnitProduct, Unit>(unitProduct.Id, unit.Id);
                POCCacheAdapter.Instance.SetDependency<Product, UnitProduct>(unitProduct.Product.Id, unitProduct.Id);
            });
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
