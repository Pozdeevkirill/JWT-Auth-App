using AuthApp.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthApp.DAL.Interfaces
{
    public interface IRoleRepository : IBaseRepository<Role>
    {
        public Role GetByName(string name);
    }

}
