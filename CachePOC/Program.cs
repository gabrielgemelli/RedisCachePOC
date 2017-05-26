using System.Collections.Generic;
using CachePOC.Controllers;
using CachePOC.ExternalModels;
using CachePOC.Models;
using CachePOC.Synchronizers;
using static System.Console;

namespace CachePOC
{
    class Program
    {
        static UnitController _unitControler = new UnitController();
        static ProductSynchronizer _productSynchronizer = new ProductSynchronizer();

        static void Main(string[] args)
        {
            var unit = InitializeData();

            WriteLine();
            WriteLine("Sincronizando produto");
            WriteLine();

            _productSynchronizer.Sync(1);

            var cachedUnit = POCCacheAdapter.Instance.Get<Unit>(unit.Id);

            CheckIfUnitIsAviable(cachedUnit);

            CheckIfProductIsAviable(2);

            WriteLine();
            WriteLine("Limpando Cache");
            WriteLine();

            ClearCache();

            WriteLine();
            WriteLine("Cache Limpa");
            WriteLine();

            ReadKey();
        }

        private static void CheckIfProductIsAviable(int productId)
        {
            var product = POCCacheAdapter.Instance.Get<Product>(productId);
            if (product == null)
            {
                WriteLine("Produto {0} não encontrado", productId);
            }
            else
            {
                WriteLine("Produto {0} encontrado", productId);
            }
        }

        private static void CheckIfUnitIsAviable(Unit cachedUnit)
        {
            WriteLine();

            if (cachedUnit == null)
            {
                WriteLine("Unidade foi removida");
            }
            else
            {
                WriteLine("Unidade ainda esta no Cache");
            }

            WriteLine();
        }

        private static Unit InitializeData()
        {
            ClearCache();

            Unit unit = GetUnit();

            _unitControler.Post(unit);

            WriteLine();
            WriteLine("Unidade adicionada no cache");
            WriteLine();

            return unit;
        }

        private static Unit GetUnit()
        {
            var unit = new Unit
            {
                Id = 1,
                Name = "Unidade de teste"
            };

            unit.Products = new List<UnitProduct>();

            for (int i = 1; i <= 300; i++)
            {
                var unitProduct = new UnitProduct();

                unitProduct.UnitId = unit.Id;
                unitProduct.Id = i;
                unitProduct.Product = GetProduct(i);

                unit.Products.Add(unitProduct);
            }

            return unit;
        }

        private static void ClearCache()
        {
            POCCacheAdapter.Instance.DisableLogging();
            POCCacheAdapter.Instance.Clear();
            POCCacheAdapter.Instance.EnableLogging();
        }

        private static Product GetProduct(long id)
        {
            return new Product
            {
                Id = id,
                Name = string.Format("Produto de teste - {0}", id)
            };
        }
    }
}
