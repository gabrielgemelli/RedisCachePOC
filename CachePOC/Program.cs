using System.Linq;
using System.Collections.Generic;
using System.Diagnostics;
using CachePOC.Controllers;
using CachePOC.ExternalModels;
using CachePOC.Models;
using CachePOC.Synchronizers;
using static System.Console;
using static CachePOC.ConsoleHelper;

namespace CachePOC
{
    class Program
    {
        static UnitController _unitControler = new UnitController();
        static DietController _dietController = new DietController();
        static DietGroupController _dietGroupController = new DietGroupController();

        static ProductSynchronizer _productSynchronizer = new ProductSynchronizer();

        static void Main(string[] args)
        {
            POCCacheAdapter.Instance.DisableLogging();

            //Testando Unidate com sincronização de produto
            TestUnitWithProductSync();

            //Testando Dieta com sincronização de producto
            TestDietWithProductSync();

            //Testando Dieta com update de Unidate
            TestDietWithUnitUpdate();

            //Testando Grupo de dieta com sincronização de product
            TesteDietGroupWithProductSync();

            ReadKey();
        }

        private static void TesteDietGroupWithProductSync()
        {
            var diets = new List<Diet>();

            var diet1 = GetDiet(1, 5, 10);
            var diet2 = GetDiet(2, 5, 10);
            var diet3 = GetDiet(3, 5, 10);
            var diet4 = GetDiet(4, 5, 10);
            var diet5 = GetDiet(5, 5, 10);

            var dietGroup1 = GetDietGroup(1, diet1, diet2, diet3, diet4, diet5);
            var dietGroup2 = GetDietGroup(2, diet1, diet2);

            _dietGroupController.Post(dietGroup1);
            _dietGroupController.Post(dietGroup2);

            _productSynchronizer.Sync(4);

            CheckIfDietWasRemoved(diet1);
            CheckIfDietWasRemoved(diet2);
            CheckIfDietWasRemoved(diet3);
            CheckIfDietWasRemoved(diet4);
            CheckIfDietWasRemoved(diet5);

            CheckIfDietGroupWasRemoved(dietGroup1);
            CheckIfDietGroupWasRemoved(dietGroup2);

            ClearCache();
        }

        private static void CheckIfDietGroupWasRemoved(DietGroup dietGroup)
        {
            WriteLine();
            WriteLine("<Checando se o grupo de dieta {0} foi removido>", dietGroup.Id);

            WriteLine("\t<Buscando grupo de dieta {0} no Cache>", dietGroup.Id);

            var stopwatch = Stopwatch.StartNew();
            var cachedDiet = POCCacheAdapter.Instance.Get<DietGroup>(dietGroup.Id);
            stopwatch.Stop();

            WriteLine("\t\ttempo - {0}", stopwatch.Elapsed.ToString());
            WriteLine("\t</Buscando grupo de dieta {0} no Cache>", dietGroup.Id);

            if (cachedDiet == null)
            {
                WriteSuccessMessage("Grupo de dieta {0} foi Removido", dietGroup.Id);
            }
            else
            {
                WriteErrorMessage("Grupo de dieta {0} não foi removido", dietGroup.Id);
            }

            WriteLine("</Checando se o grupo de dieta {0} foi removido>", dietGroup.Id);
        }

        private static void TestDietWithUnitUpdate()
        {
            var diet1 = GetDiet(1, 2, 10);
            var diet2 = GetDiet(2, 2, 11);

            AddDietToCache(diet1);
            AddDietToCache(diet2);

            UpdateUnit(11);

            CheckIfDietWasRemoved(diet2);
            CheckIfDietIsAvailable(diet1);

            ClearCache();
        }

        private static void TestDietWithProductSync()
        {
            var diet = GetDiet(1, 2, 10);

            AddDietToCache(diet);

            SyncProduct(1);

            CheckIfDietWasRemoved(diet);

            ClearCache();
        }

        private static void TestUnitWithProductSync()
        {
            var unit = GetUnit(1, 30);

            AddUnitToCache(unit);

            SyncProduct(1);

            CheckIfUnitWasRemoved(unit);

            CheckIfProductIsAvailable(2);

            ClearCache();
        }

        private static void CheckIfDietIsAvailable(Diet diet)
        {
            WriteLine();
            WriteLine("<Checando se dieta {0} está na cache>", diet.Id);

            WriteLine("\t<Buscando dieta {0} no Cache>", diet.Id);

            var dietTime = Stopwatch.StartNew();
            var cachedDiet = POCCacheAdapter.Instance.Get<Diet>(diet.Id);
            dietTime.Stop();

            WriteLine("\t\ttempo - {0}", dietTime.Elapsed.ToString());
            WriteLine("\t</Buscando dieta {0} no Cache>", diet.Id);

            if (cachedDiet == null)
            {
                WriteSuccessMessage("Dieta {0} está na cache", diet.Id);
            }
            else
            {
                WriteErrorMessage("Dieta {0} não está na cache", diet.Id);
            }

            WriteLine("</Checando se dieta {0} está na cache>", diet.Id);
        }

        private static void CheckIfDietWasRemoved(Diet diet)
        {
            WriteLine();
            WriteLine("<Checando se dieta {0} foi removida>", diet.Id);

            WriteLine("\t<Buscando dieta {0} no Cache>", diet.Id);

            var dietTime = Stopwatch.StartNew();
            var cachedDiet = POCCacheAdapter.Instance.Get<Diet>(diet.Id);
            dietTime.Stop();

            WriteLine("\t\ttempo - {0}", dietTime.Elapsed.ToString());
            WriteLine("\t</Buscando dieta {0} no Cache>", diet.Id);

            if (cachedDiet == null)
            {
                WriteSuccessMessage("Dieta {0} foi Removida", diet.Id);
            }
            else
            {
                WriteErrorMessage("Dieta {0} não foi removida", diet.Id);
            }

            WriteLine("</Checando se dieta {0} foi removida>", diet.Id);
        }

        private static void AddDietToCache(Diet diet)
        {
            WriteLine();
            WriteLine("<Adicionando dieta {0} no Cache>", diet.Id);

            var dietTime = Stopwatch.StartNew();
            _dietController.Post(diet);
            dietTime.Stop();

            WriteLine("\ttempo - {0}", dietTime.Elapsed.ToString());
            WriteLine("<Adicionando dieta {0} no Cache>", diet.Id);
            WriteLine();
        }

        private static void ClearCache()
        {
            WriteLine();
            WriteLine("<Limpando cache>");

            var clearCacheTime = Stopwatch.StartNew();
            POCCacheAdapter.Instance.Clear();
            clearCacheTime.ToString();

            WriteLine("\ttempo - {0}", clearCacheTime.Elapsed.ToString());
            WriteLine("</Limpando cache>");

            WriteLine();
        }

        private static void SyncProduct(long id)
        {
            WriteLine();
            WriteLine("<Sincronizando produto {0}>", id);

            var productSyncTime = Stopwatch.StartNew();
            _productSynchronizer.Sync(id);
            productSyncTime.Stop();

            WriteLine("\ttempo - {0}", productSyncTime.Elapsed.ToString());
            WriteLine("</Sincronizando produto {0}>", id);
            WriteLine();
        }

        private static void CheckIfProductIsAvailable(int productId)
        {
            WriteLine();
            WriteLine("<Checando se produto {0} está na cache>", productId);

            WriteLine("\t<Buscando produto {0} no Cache>", productId);

            var productTime = Stopwatch.StartNew();
            var product = POCCacheAdapter.Instance.Get<Product>(productId);
            productTime.Stop();

            WriteLine("\t\ttempo - {0}", productTime.Elapsed.ToString());
            WriteLine("\t</Buscando produto {0} no Cache>", productId);

            if (product == null)
            {
                WriteErrorMessage("Produto {0} não encontrado", productId);
            }
            else
            {
                WriteSuccessMessage("Produto {0} encontrado", productId);
            }

            WriteLine("</Checando se produto {0} está na cache>", productId);
        }

        private static void CheckIfUnitWasRemoved(Unit unit)
        {
            WriteLine("<Checando se a unida {0} foi removida>", unit.Id);

            WriteLine("\t<Buscando Unidade {0} no Cache>", unit.Id);

            var unitTime = Stopwatch.StartNew();
            var cachedUnit = POCCacheAdapter.Instance.Get<Unit>(unit.Id);
            unitTime.Stop();

            WriteLine("\t\ttempo - {0}", unitTime.Elapsed.ToString());
            WriteLine("\t</Buscando Unidade {0} no Cache>", unit.Id);

            if (cachedUnit == null)
            {
                WriteSuccessMessage("Unidade {0} foi removida", unit.Id);
            }
            else
            {
                WriteErrorMessage("Unidade {0} ainda está no Cache", unit.Id);
            }

            WriteLine("</Checando se a unida {0} foi removida>", unit.Id);
        }

        private static void AddUnitToCache(Unit unit)
        {
            WriteLine();
            WriteLine("<Adicionando unidade no cache>");

            var addingUnitTime = Stopwatch.StartNew();
            _unitControler.Post(unit);
            addingUnitTime.Stop();

            WriteLine("\ttempo - {0}", addingUnitTime.Elapsed.ToString());
            WriteLine("</Adicionando unidade no cache>");
            WriteLine();
        }

        private static void UpdateUnit(long id)
        {
            var unit = POCCacheAdapter.Instance.Get<Unit>(id);

            WriteLine();
            WriteLine("<Atualizando unidade {0} no cache>", id);

            var updatingUnit = Stopwatch.StartNew();
            _unitControler.Put(unit);
            updatingUnit.Stop();

            WriteLine("\ttempo - {0}", updatingUnit.Elapsed.ToString());
            WriteLine("</Atualizando unidade {0} no cache>", id);
            WriteLine();
        }

        private static DietGroup GetDietGroup(long id, params Diet[] diets)
        {
            var dietGroup = new DietGroup();

            dietGroup.Id = id;
            dietGroup.Diets = diets.ToList();

            return dietGroup;
        }

        private static Diet GetDiet(long id, int numberOfProducts, int numberOfUnits)
        {
            var diet = new Diet
            {
                Id = id,
                Name = string.Format("Dieta de teste {0}", id)
            };

            diet.Products = new List<DietProduct>();
            diet.Units = new List<DietUnit>();

            for (int i = 1; i <= numberOfProducts; i++)
            {
                var dietProduct = new DietProduct();

                dietProduct.Id = i;
                dietProduct.DietId = diet.Id;
                dietProduct.ProductId = i;
                dietProduct.Product = GetProduct(i);

                diet.Products.Add(dietProduct);
            }

            for (int i = 1; i <= numberOfUnits; i++)
            {
                var dietUnit = new DietUnit();

                dietUnit.Id = i;
                dietUnit.DietId = diet.Id;
                dietUnit.UnitId = i;
                dietUnit.Unit = GetUnit(i, 10);

                diet.Units.Add(dietUnit);
            }

            return diet;
        }

        private static Unit GetUnit(long id, int numberOfProducts)
        {
            var unit = new Unit
            {
                Id = id,
                Name = string.Format("Unidade de teste {0}", id)
            };

            unit.Products = new List<UnitProduct>();

            for (int i = 1; i <= numberOfProducts; i++)
            {
                var unitProduct = new UnitProduct();

                unitProduct.UnitId = unit.Id;
                unitProduct.Id = i;
                unitProduct.Product = GetProduct(i);

                unit.Products.Add(unitProduct);
            }

            return unit;
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
