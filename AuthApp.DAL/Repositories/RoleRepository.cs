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
    public class RoleRepository : IRoleRepository
    {
        AppDbContext db;
        public RoleRepository(AppDbContext _db)
        {
            db = _db;
        }

        public void Create(Role model)
        {
            if (model != null)
            {
                db.Roles.Add(model);
            }
            else
                return;
        }

        public void Delete(int id)
        {
            if (id >= 0)
            {
                var role = db.Roles
                    .Include(r => r.Users)
                    .FirstOrDefault(u => u.Id == id);
                db.Roles.Remove(role);
            }
            else
                return;
        }

        public IEnumerable<Role> GetAll()
        {
            return db.Roles.Include(r => r.Users);
        }

        public Role GetById(int id)
        {
            if (id >= 0)
            {
                var role = db.Roles
                    .Include(r => r.Users)
                    .FirstOrDefault(r => r.Id == id);
                return role;
            }
            else
                return null;
        }

        public Role GetByName(string name)
        {
            if (name != string.Empty)
            {
                return db.Roles
                    .Include(r => r.Users)
                    .FirstOrDefault(r => r.Name == name);
            }
            else
                return null;
        }

        public Role Update(Role model)
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
