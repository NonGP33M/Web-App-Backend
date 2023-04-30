using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using System.Text;
using server.DTO.Auth;
using server.DTO.User;
using server.data;
using server.model;


namespace server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly WebAppDbContext WebAppDbContext;
        private readonly IConfiguration Configuration;

        public AuthController(WebAppDbContext WebAppDbContext, IConfiguration Configuration)
        {
            this.WebAppDbContext = WebAppDbContext;
            this.Configuration = Configuration;
        }

        [HttpPost("Register")]
        public async Task<HttpStatusCode> Register(AddUserDTO User)
        {
            if (User.Password.Length == 0 || User.Username.Length == 0)
            {
                return HttpStatusCode.BadRequest;
            }
            try
            {
                Guid myuuid = Guid.NewGuid();
                string myuuidAsString = myuuid.ToString();
                string passwordHash = BCrypt.Net.BCrypt.HashPassword(User.Password);
                var newUser = new UserModel
                {
                    UserId = myuuidAsString,
                    Username = User.Username,
                    Password = passwordHash,
                    UserImg = "nullUser.png",
                    FirstName = User.FirstName,
                    LastName = User.LastName,
                    Tel = User.Tel,
                    Score = 0,
                    Success = 0,
                    Failed = 0
                };
                WebAppDbContext.Users.Add(newUser);
                await WebAppDbContext.SaveChangesAsync();
                return HttpStatusCode.Created;
            }
            catch
            {
                return HttpStatusCode.InternalServerError;
            }
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login(LoginDTO User)
        {
            var foundUser = await WebAppDbContext.Users.FirstOrDefaultAsync(e => e.Username == User.Username);
            if (foundUser == null)
            {
                return BadRequest("Invalid user request!!!");
            }
            bool match = BCrypt.Net.BCrypt.Verify(User.Password, foundUser.Password);
            if (match)
            {
                var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["JWT:Key"]));
                var signinCredentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);
                var claims = new[]{
                    new Claim("UserId", foundUser.UserId.ToString()),
                    new Claim("Username", foundUser.Username.ToString()),
                    new Claim("Score", foundUser.Score.ToString()),
                    new Claim("Tel", foundUser.Tel.ToString())
                    };
                var tokeOptions = new JwtSecurityToken(
                    issuer: Configuration["JWT:Issuer"],
                    audience: Configuration["JWT:Audience"],
                    claims: claims,
                    expires: DateTime.Now.AddMinutes(5),
                    signingCredentials: signinCredentials);
                var tokenString = new JwtSecurityTokenHandler().WriteToken(tokeOptions);
                return Ok(new TokenDTO
                {
                    Token = tokenString,
                });
            }
            else
            {
                return Unauthorized("Wrong Password");
            }
        }
    }
}