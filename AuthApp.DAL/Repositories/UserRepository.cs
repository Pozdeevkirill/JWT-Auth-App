using AuthApp.DAL.Data;
using AuthApp.DAL.Interfaces;
using AuthApp.DAL.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthApp.DAL.Repositories
{
    public class UserRepository : IUserRepository
    {
        AppDbContext db;
        public UserRepository(AppDbContext _db)
        {
            db = _db;
        }

        public void Create(User model)
        {
            if (model != null)
            {
                db.Users.Add(model);
            }
            else
                return;
        }

        public void Delete(int id)
        {
            if (id >= 0)
            {
                var user = db.Users
                    .Include(u => u.Role)
                    .FirstOrDefault(u => u.Id == id);
                db.Users.Remove(user);
            }
            else
                return;
        }

        public IEnumerable<User> FindByName(string name)
        {
            if (name != string.Empty)
            {
                return db.Users
                    .Include(u => u.Role)
                    .Where(u => u.Name == name);
            }
            else
                return null;
        }

        public IEnumerable<User> GetAll()
        {
            return db.Users.Include(u => u.Role);
        }

        public User GetById(int id)
        {
            if (id >= 0)
            {
                var user = db.Users
                    .Include(u => u.Role)
                    .FirstOrDefault(u => u.Id == id);
                return user;
            }
            else
                return null;
        }

        public User GetByLogin(string login)
        {
            if (login != string.Empty)
            {
                return db.Users
                    .Include(u => u.Role)
                    .FirstOrDefault(u => u.Login == login);
            }
            else
                return null;
        }

        public User GetByName(string name)
        {
            if (name != string.Empty)
            {
                return db.Users
                    .Include(u => u.Role)
                    .FirstOrDefault(u => u.Name == name);
            }
            else
                return null;
        }

        public User Update(User model)
        {
            if (model != null)
            {
                db.Entry(model).State = EntityState.Modified;
                return model;
            }
            else
                return null;
        }
    }
}
