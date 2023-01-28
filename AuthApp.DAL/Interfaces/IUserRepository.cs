using AuthApp.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthApp.DAL.Interfaces
{
    public interface IUserRepository : IBaseRepository<User>
    {
        public IEnumerable<User> FindByName(string name);
        public User GetByName(string name);
        public User GetByLogin(string login);
    }
}
