using CachePOC.Models;

namespace CachePOC.Controllers
{
    public class ProductController
    {
        public Product Get(long id)
        {
            var product = POCRedisCache.Instance.Get<Product>(id);

            //if (produto != null)
            //{
            //    var category = CarregaCategoria(produto.Categoria.Id);

            //    if (category != null)
            //    {
            //        produto.Category = category;
            //    }

            //    var manufacturer = CarregaFabricante(produto.Manufacturer.Id);

            //    if (manufacturer != null)
            //    {
            //        produto.Manufacturer = manufacturer;
            //    }
            //}

            return product;
        }

        public void Post(Product product)
        {
            POCRedisCache.Instance.Remove<Product>(product.Id);

            POCRedisCache.Instance.Add(product, product.Id);
        }
    }
}
