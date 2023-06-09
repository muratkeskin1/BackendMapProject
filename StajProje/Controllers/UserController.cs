using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using StajProje.Helper;
using StajProje.Model;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace StajProje.Controllers
{
    [ApiController]
    [Route("user")]
    [Authorize]
    [EnableCors]
    public class UserController : ControllerBase
    {
        private readonly ILogger<UserController> _logger;
        private readonly DbHelper _db;
        public UserController(ILogger<UserController> logger, ApplicationDbContext context)
        {
            _logger = logger;
            _db = new DbHelper(context);
        }
        [HttpPost]
        public IActionResult UserAdd([FromForm] UserLogin user) {
            //_logger.LogInformation(user.age.ToString() + " " +user.name+" add");
            _logger.LogInformation("new user added by "+HttpContext.Connection.RemoteIpAddress);
            UserLogin userGet = _db.GetUserByEmail(user.email);
            if (userGet!=null)
            {
                return BadRequest("Bu email ile kayıt var.");
            }
            _db.AddUser(user);
            return Ok();
        }
        [HttpPost("delete/{id}")]
        public IActionResult UserDelete([FromRoute] int id)
        {
            _logger.LogInformation(id + " " + " deleted by " + HttpContext.Connection.RemoteIpAddress);
            _db.DeleteUser(id);
            return Ok();
        }
        [HttpPost("/update")]
        public IActionResult UpdateUser([FromForm] UserLogin user)
        {
            _logger.LogInformation(user.id + " is updated by " + HttpContext.Connection.RemoteIpAddress);

            _db.Update(user);
            return Ok();
        }
        [HttpGet]
        public IActionResult home()
        {
            _logger.LogWarning(DateTime.Now.ToString() +
                " " + HttpContext.Connection.RemoteIpAddress + " getall");
            return Ok(_db.GetUsers());
        }
        [HttpGet("/{name}")]
        public IActionResult getUserData([FromRoute] string name)
        {
            _logger.LogDebug( " " + name + " getuser");
            return Ok(_db.GetUser(name,""));
        }
        [HttpPost("/login/{email}/{pw}")]
        [AllowAnonymous]
        public async Task<IActionResult> LoginAsync(string email,string pw)
        {
            _logger.LogWarning(email  + " " + pw +
                " logged in "+ HttpContext.Connection.RemoteIpAddress);
            //db check yapılacak
            //session açılacak
            UserLogin user = _db.GetUser(email, pw);
            if (user!=null)
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var key =Encoding.UTF8.GetBytes("Secret phaseSecret phaseSecret phaseSecret phase");
                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(new Claim[]
                    {
                    new Claim(ClaimTypes.Email, user.email)
                    }),
                    Expires = DateTime.UtcNow.AddDays(7),
                    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256)
                };
                var token = tokenHandler.CreateToken(tokenDescriptor);
                HttpContext.Session.SetString("sessionId", tokenHandler.WriteToken(token));
                return Ok(new { token=tokenHandler.WriteToken(token)});
            }
            return BadRequest("not found user");
       
        }
    }
}
