using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CachePOC.Models;
using CachePOC.Repositories;

namespace CachePOC.Controllers
{
    public class DietGroupController
    {
        private readonly DietGroupRepository _dietGroupRepository = new DietGroupRepository();

        public void Post(DietGroup dietGroup)
        {
            _dietGroupRepository.Insert(dietGroup);
        }
    }
}
