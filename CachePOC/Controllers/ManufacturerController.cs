using CachePOC.Models;

namespace CachePOC.Controllers
{
    public class ManufacturerController
    {
        public Manufacturer Get(long id)
        {
            return POCRedisCache.Instance.Get<Manufacturer>(id);
        }

        public void Post(Manufacturer manufacturer)
        {
            POCRedisCache.Instance.Remove<Manufacturer>(manufacturer.Id);

            POCRedisCache.Instance.Add(manufacturer, manufacturer.Id);
        }

        public void Put(long id, string name)
        {
            var category = this.Get(id);

            category.Name = name;

            POCRedisCache.Instance.Add(category, category.Id);
        }
    }
}
