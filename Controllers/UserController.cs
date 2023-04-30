using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net;
using server.DTO.User;
using server.data;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Authorization;
using System;
using System.IO;
using AutoMapper;
using server.model;


namespace server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly WebAppDbContext WebAppDbContext;
        private readonly IConfiguration Configuration;
        private readonly IMapper Mapper;
        public UserController(WebAppDbContext WebAppDbContext, IConfiguration Configuration, IMapper Mapper)
        {
            this.WebAppDbContext = WebAppDbContext;
            this.Configuration = Configuration;
            this.Mapper = Mapper;
        }

        [HttpGet("[action]")]
        public async Task<ActionResult<List<UserInfoDTO>>> GetTopUser()
        {
            var List = await WebAppDbContext.Users.Select(
                s => new UserInfoDTO
                {
                    Username = s.Username,
                    UserImg = s.UserImg,
                    Score = s.Score,
                    FirstName = s.FirstName,
                    LastName = s.LastName,
                    Tel = s.Tel,
                    Success = s.Success,
                    Failed = s.Failed
                }
            ).ToListAsync();

            var sorted = List.OrderByDescending(e => e.Score).Take(10).ToList();

            if (sorted.Count < 0)
            {
                return NotFound();
            }
            else
            {
                return sorted;
            }
        }

        [HttpGet("[action]/{UserId}")]
        public async Task<ActionResult<UserInfoAndPlaceDTO>> GetMyPlace(string UserId)
        {
            var List = await WebAppDbContext.Users.Select(
                s => new UserModel
                {
                    UserId = s.UserId,
                    Username = s.Username,
                    UserImg = s.UserImg,
                    Score = s.Score,
                    FirstName = s.FirstName,
                    LastName = s.LastName,
                    Tel = s.Tel,
                    Success = s.Success,
                    Failed = s.Failed
                }
            ).ToListAsync();

            var sorted = List.OrderByDescending(e => e.Score).ToList();
            int place = sorted.FindIndex(e => e.UserId == UserId);

            UserDTO User = await WebAppDbContext.Users.Select(s => new UserDTO
            {
                UserId = s.UserId,
                Username = s.Username,
                Password = s.Password,
                FirstName = s.FirstName,
                LastName = s.LastName,
                Tel = s.Tel,
                Score = s.Score,
                UserImg = s.UserImg,
                Success = s.Success,
                Failed = s.Failed
            }).FirstOrDefaultAsync(s => s.UserId == UserId);
            if (User == null)
            {
                return NotFound();
            }
            else
            {
                var userInfo = new UserInfoAndPlaceDTO();
                userInfo.Place = place + 1;
                userInfo.Username = User.Username;
                userInfo.Score = User.Score;
                userInfo.UserImg = User.UserImg;
                userInfo.FirstName = User.FirstName;
                userInfo.LastName = User.LastName;
                userInfo.Tel = User.Tel;
                userInfo.Success = User.Success;
                userInfo.Failed = User.Failed;
                return Ok(userInfo);
            }
        }

        [HttpPatch("[action]/{UserId}")]
        [Authorize]
        public async Task<HttpStatusCode> UpdateUser(string UserId, [FromForm] UpdateUserDTO updateUser)
        {
            var foundUser = await WebAppDbContext.Users.FindAsync(UserId);
            if (User is null)
            {
                return HttpStatusCode.NotFound;
            }

            try
            {
                if (foundUser.UserImg != "nullUser.png" && updateUser.Image.FileName != "")
                {
                    var oldFilepath = Path.Combine(Directory.GetCurrentDirectory(), "static", foundUser.UserImg);
                    FileInfo oldFile = new FileInfo(oldFilepath);
                    oldFile.Delete();
                }
                foundUser.FirstName = updateUser.FirstName;
                foundUser.LastName = updateUser.LastName;
                foundUser.Tel = updateUser.Tel;
                if (updateUser.Image.FileName != "")
                {
                    foundUser.UserImg = updateUser.Image.FileName.ToString();
                    var filepath = Path.Combine(Directory.GetCurrentDirectory(), "static", updateUser.Image.FileName);
                    await updateUser.Image.CopyToAsync(new FileStream(filepath, FileMode.Create));
                }
                await WebAppDbContext.SaveChangesAsync();
                return HttpStatusCode.OK;
            }
            catch (Exception err)
            {
                Console.WriteLine(err);
                return HttpStatusCode.BadRequest;
            }
        }

        [HttpGet("[action]/{UserId}")]
        // [Authorize]
        public async Task<IActionResult> GetUserById(string UserId)
        {
            // string authHeader = Request.Headers["Authorization"];
            // string[] lstAuthHeader = authHeader.Split(" ");
            // var token = lstAuthHeader[1];
            // JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
            // JwtSecurityToken decodedToken = tokenHandler.ReadJwtToken(token);
            // var claims = decodedToken.Claims;
            // string UserId = claims.FirstOrDefault(c => c.Type == "UserId")?.Value;
            UserDTO User = await WebAppDbContext.Users.Select(s => new UserDTO
            {
                UserId = s.UserId,
                Username = s.Username,
                Password = s.Password,
                FirstName = s.FirstName,

                LastName = s.LastName,
                Tel = s.Tel,
                Score = s.Score,
                UserImg = s.UserImg,
                Success = s.Success,
                Failed = s.Failed
            }).FirstOrDefaultAsync(s => s.UserId == UserId);
            if (User == null)
            {
                return NotFound();
            }
            else
            {
                var userInfo = new UserInfoDTO();
                userInfo.Username = User.Username;
                userInfo.Score = User.Score;
                userInfo.UserImg = User.UserImg;
                userInfo.FirstName = User.FirstName;
                userInfo.LastName = User.LastName;
                userInfo.Tel = User.Tel;
                userInfo.Success = User.Success;
                userInfo.Failed = User.Failed;
                return Ok(userInfo);
            }
        }
    }
}