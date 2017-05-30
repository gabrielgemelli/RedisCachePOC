using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CachePOC.ExternalModels;
using CachePOC.Models;

namespace CachePOC.Repositories
{
    public class DietProductRepository
    {
        public void Insert(DietProduct dietProduct)
        {
            AddToCache(dietProduct);
        }

        public void Update(DietProduct dietProduct)
        {
            AddToCache(dietProduct);
        }

        private static void AddToCache(DietProduct dietProduct)
        {
            POCCacheAdapter.Instance.Add(dietProduct, dietProduct.Id);
            POCCacheAdapter.Instance.Add(dietProduct.Product, dietProduct.ProductId);

            POCCacheAdapter.Instance.SetDependency<Diet, DietProduct>(dietProduct.DietId, dietProduct.Id);

            POCCacheAdapter.Instance.SetDependency<DietProduct, Diet>(dietProduct.Id, dietProduct.DietId);
            POCCacheAdapter.Instance.SetDependency<Product, DietProduct>(dietProduct.ProductId, dietProduct.Id);
        }
    }
}
