using CachePOC.ExternalModels;

namespace CachePOC.Synchronizers
{
    public class ProductSynchronizer
    {
        public void Sync(long id)
        {
            POCCacheAdapter.Instance.Remove<Product>(id);
        }
    }
}
