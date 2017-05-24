using CachePOC.Models;

namespace CachePOC.Controllers
{
    public class CategoryController
    {
        public Category Get(long id)
        {
            return POCRedisCache.Instance.Get<Category>(id);
        }

        public void Post(Category category)
        {
            POCRedisCache.Instance.Remove<Category>(category.Id);

            POCRedisCache.Instance.Add(category, category.Id);
        }

        public void Put(long id, string name)
        {
            var category = this.Get(id);

            category.Name = name;

            POCRedisCache.Instance.Add(category, category.Id);
        }
    }
}
