using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CachePOC.Models;

namespace CachePOC.Repositories
{
    public class DietRepository
    {
        private readonly DietUnitRepository _dietUnitRepository = new DietUnitRepository();
        private readonly DietProductRepository _dietProductRepository = new DietProductRepository();

        public void Insert(Diet diet)
        {
            POCCacheAdapter.Instance.Add(diet, diet.Id);

            diet.Units?.ForEach(_dietUnitRepository.Insert);
            diet.Products?.ForEach(_dietProductRepository.Insert);
        }

        public void Update(Diet diet)
        {
            POCCacheAdapter.Instance.Add(diet, diet.Id);

            diet.Units?.ForEach(_dietUnitRepository.Update);
            diet.Products?.ForEach(_dietProductRepository.Update);
        }
    }
}
