using System;
using CachePOC.ExternalModels;
using CachePOC.Models;

namespace CachePOC.Repositories
{
    internal class UnitProductRepository
    {
        public void Insert(UnitProduct unitProduct)
        {
            POCCacheAdapter.Instance.Add(unitProduct, unitProduct.Id);
            POCCacheAdapter.Instance.Add(unitProduct.Product, unitProduct.Product.Id);

            POCCacheAdapter.Instance.SetDependency<Unit, UnitProduct>(unitProduct.UnitId, unitProduct.Id);

            POCCacheAdapter.Instance.SetDependency<UnitProduct, Unit>(unitProduct.Id, unitProduct.UnitId);
            POCCacheAdapter.Instance.SetDependency<Product, UnitProduct>(unitProduct.Product.Id, unitProduct.Id);
        }
    }
}