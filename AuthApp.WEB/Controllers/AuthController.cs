using AuthApp.BAL.Interfaces;
using AuthApp.BAL.ModelsDTO;
using AuthApp.WEB.Commons;
using AuthApp.WEB.Models;
using AuthApp.WEB.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace AuthApp.WEB.Controllers
{
    [ApiController]
    [Route("api/")]
    public class AuthController : ControllerBase
    {
        IUserServices userServices;
        private readonly ILogger<AuthController> _logger;

        public AuthController(IUserServices us, ILogger<AuthController> logger)
        {
            userServices = us;
            _logger = logger;
        }

        [HttpPost]
        [Route("Register")]
        public IActionResult Register([FromBody] RegisterVM registerVM)
        {
            var user = userServices.FindByName(registerVM.Name).ToList();
            if (user.Count <= 0)
            {
                UserDTO userDTO = new()
                {
                    Name = registerVM.Name,
                    Login = registerVM.Login,
                    Password = registerVM.Password,
                    Role = "user",
                };

                if (registerVM.Password != registerVM.ConfirmPassword)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError,
                        new Response<RegisterVM> { Status = "Error", Message = "Пароли не совпадают!" });
                }

                var result = userServices.Create(userDTO);

                if (result != null)
                {
                    return StatusCode(StatusCodes.Status200OK,
                        new Response<RegisterVM> { Status = "OK", Message = "Пользователь успешно создан!" });
                }
                else
                {
                    return StatusCode(StatusCodes.Status500InternalServerError,
                        new Response<RegisterVM> { Status = "Error", Message = "При создании пользователя возникла ошибка!" });
                }
            }
            else
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new Response<RegisterVM> { Status = "Error", Message = "Пользователь с таким именем уже зарегестрирован!" });
            }
        }

        [HttpPost]
        [Route("Login")]
        public IActionResult Login([FromBody] LoginVM loginVM)
        {
            var user = userServices.GetByLogin(loginVM.Login);
            if (user != null && loginVM.Login == user.Login && loginVM.Password == user.Password)
            {
                var claims = new List<Claim> { new Claim(ClaimTypes.Name, loginVM.Login)};

                var jwt = new JwtSecurityToken(
                    issuer: AuthOptions.ISSUER,
                    audience: AuthOptions.AUDIENCE,
                    claims: claims,
                    expires: DateTime.UtcNow.Add(TimeSpan.FromMinutes(2)),
                    signingCredentials: new SigningCredentials(AuthOptions.GetSymmetricSecurityKey(), SecurityAlgorithms.HmacSha256));
                var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);


                UserLogin response = new()
                {
                    access_token = encodedJwt,
                    username = loginVM.Login
                };
                return StatusCode(StatusCodes.Status200OK,
                    new Response<UserLogin> { Status="OK", Message="Успешная авторизация", Data=response});
            }
            else
            {
                return StatusCode(StatusCodes.Status401Unauthorized,
                    new Response<LoginVM> { Status = "Error", Message = "Неверный логин или пароль"});
            }
        }

        [Authorize]
        [HttpPost]
        [Route("GetAll")]
        public IActionResult GetAll()
        {
            var data = userServices.GetAll();
            return StatusCode(StatusCodes.Status200OK, new Response<IEnumerable<UserDTO>> { Status="OK", Data = data});
        }

        [HttpPost]
        [Route("GetAllNonAuth")]
        public IActionResult GetAllNonAuth()
        {
            var data = userServices.GetAll();
            return StatusCode(StatusCodes.Status200OK, new Response<IEnumerable<UserDTO>> { Status = "OK", Data = data });
        }
    }
}
