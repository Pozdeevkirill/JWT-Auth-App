using AuthApp.BAL.Interfaces;
using AuthApp.BAL.ModelsDTO;
using AuthApp.DAL.Interfaces;
using AuthApp.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthApp.BAL.Services
{
    public class UserServices : IUserServices
    {
        IUnitOfWork db;

        public UserServices(IUnitOfWork _db)
        {
            db = _db;
        }

        public UserDTO Create(UserDTO userDTO)
        {
            try
            {
                if(userDTO != null)
                {
                    User user = new()
                    {
                        Name = userDTO.Name,
                        Login = userDTO.Login,
                        Password = userDTO.Password,
                    };

                    user.Role = db.RoleRepository.GetByName(userDTO.Role);
                    db.UserRepository.Create(user);
                    db.SaveAsync();

                    return userDTO;
                }
                else
                {
                    return null;
                }
            }
            catch
            {
                return null;
            }
        }

        public IEnumerable<UserDTO> FindByName(string name)
        {
            try
            {
                if(name != string.Empty)
                {
                    var users = db.UserRepository.FindByName(name);
                    List<UserDTO> usersDTO = new();

                    foreach (var _user in users)
                    {
                        UserDTO _userDTO = new()
                        {
                            Id = _user.Id,
                            Name = _user.Name,
                            Login = _user.Name,
                            Password = _user.Password,
                            Role = _user.Role.Name,
                        };
                        usersDTO.Add(_userDTO);
                    }
                    return usersDTO;
                }
                else
                {
                    return null;
                }
            }
            catch(Exception)
            {
                throw;
            }
        }

        public UserDTO Get(int id)
        {
            try
            {
                if(id >= 0)
                {
                    var user =  db.UserRepository.GetById(id);
                    return new()
                    {
                        Id = user.Id,
                        Name = user.Name,
                        Login = user.Login,
                        Password = user.Password,
                        Role = user.Role.Name,
                    };
                }
                else
                {
                    return null;
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public IEnumerable<UserDTO> GetAll()
        {
            try
            {
                var users = db.UserRepository.GetAll();
                List<UserDTO> usersDTO = new();

                foreach (var _user in users)
                {
                    UserDTO _userDTO = new()
                    {
                        Id = _user.Id,
                        Name = _user.Name,
                        Login = _user.Name,
                        Password = _user.Password,
                        Role = _user.Role.Name,
                    };
                    usersDTO.Add(_userDTO);
                }
                return usersDTO;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public UserDTO GetByLogin(string login)
        {
            try
            {
                if (login != string.Empty)
                {
                    var user = db.UserRepository.GetByLogin(login);

                    if(user == null)
                        return null;

                    UserDTO _userDTO = new()
                    {
                        Id = user.Id,
                        Name = user.Name,
                        Login = user.Name,
                        Password = user.Password,
                        Role = user.Role.Name,
                    };
                    return _userDTO;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public UserDTO Update(UserDTO userDTO)
        {
            try
            {
                if(userDTO != null)
                {
                    User user = new()
                    {
                        Id = userDTO.Id,
                        Name = userDTO.Name,
                        Login = userDTO.Name,
                        Password = userDTO.Password,
                        Role = db.RoleRepository.GetByName(userDTO.Role),
                    };
                    db.UserRepository.Update(user);
                    db.SaveAsync();
                    return userDTO;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
