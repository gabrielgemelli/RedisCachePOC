using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CachePOC.Models;
using CachePOC.Repositories;

namespace CachePOC.Controllers
{
    public class DietController
    {
        private readonly DietRepository _dietRepository = new DietRepository();

        public void Post(Diet diet)
        {
            _dietRepository.Insert(diet);
        }

        public void Put(Diet diet)
        {
            _dietRepository.Update(diet);
        }
    }
}
