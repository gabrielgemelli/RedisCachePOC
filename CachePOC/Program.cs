using CachePOC.Controllers;
using CachePOC.Models;
using System;

namespace CachePOC
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Criando objetos no cache");

            InitializeData();

            Console.WriteLine("Objetos criados no cache");

            long id = 1;

            WriteCacheCategory(id);

            WriteCacheManufacturer(id);

            WriteProductCategory(id);

            WriteProductManufacturer(id);

            Console.WriteLine("Atualizando a categoria no cache");

            UpdateCategory(id, "Nova Categoria 1");

            Console.WriteLine("Categoria atualizada no cache");

            Console.WriteLine("Atualizando fabricante no cache");

            UpdateManufacturer(id, "Novo Fabricante 1");

            Console.WriteLine("Fabricante atualizado no cache");

            WriteCacheCategory(id);

            WriteCacheManufacturer(id);

            WriteProductCategory(id);

            WriteProductManufacturer(id);

            Console.ReadKey();
        }

        private static void WriteProductManufacturer(long productId)
        {
            var product = new ProductController().Get(productId);

            Console.WriteLine(string.Format("Fabricante do produto {0}: {1}", productId, product.Manufacturer.Name));
        }

        private static void WriteProductCategory(long productId)
        {
            var product = new ProductController().Get(productId);

            Console.WriteLine(string.Format("Categoria do produto {0}: {1}", productId, product.Category.Name));
        }

        private static void WriteCacheManufacturer(long manufacturerId)
        {
            var manufacturer = new ManufacturerController().Get(manufacturerId);

            Console.WriteLine(string.Format("Fabricante id {0} no cache: {1}", manufacturerId, manufacturer.Name));
        }

        private static void WriteCacheCategory(long categoryId)
        {
            var category = new CategoryController().Get(categoryId);

            Console.WriteLine(string.Format("Categoria {0} no cache: {1}", categoryId, category.Name));
        }

        private static Category AddCategory(string nome)
        {
            long id = 1;

            var category = new Category(id, nome);

            new CategoryController().Post(category);

            return category;
        }

        private static void UpdateCategory(long id, string name)
        {
            new CategoryController().Put(id, name);
        }

        private static Manufacturer AddManufacturer(string name)
        {
            long id = 1;

            var manufacturer = new Manufacturer(id, name);

            new ManufacturerController().Post(manufacturer);

            return manufacturer;
        }

        private static void UpdateManufacturer(long id, string name)
        {
            new ManufacturerController().Put(id, name);
        }

        private static void AddProduct(string name, Category category, Manufacturer manufacturer)
        {
            long id = 1;

            var product = new Product(id, name, category, manufacturer);

            new ProductController().Post(product);
        }

        private static void InitializeData()
        {
            var category = AddCategory("Categoria 1");

            var manufacturer = AddManufacturer("Fabricante 1");

            AddProduct("Produto 1", category, manufacturer);
        }
    }
}
