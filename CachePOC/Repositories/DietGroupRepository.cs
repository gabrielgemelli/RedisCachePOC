using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CachePOC.Models;

namespace CachePOC.Repositories
{
    public class DietGroupRepository
    {
        private readonly DietRepository _dietRepository = new DietRepository();

        public void Insert(DietGroup dietGroup)
        {
            POCCacheAdapter.Instance.Add(dietGroup, dietGroup.Id);

            dietGroup.Diets?.ForEach(diet =>
            {
                _dietRepository.Insert(diet);

                POCCacheAdapter.Instance.SetDependency<Diet, DietGroup>(diet.Id, dietGroup.Id);
                POCCacheAdapter.Instance.SetDependency<DietGroup, Diet>(dietGroup.Id, diet.Id);
            });
        }
    }
}
