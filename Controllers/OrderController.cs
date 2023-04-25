using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net;
using server.DTO.Order;
using server.data;
using server.model;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Authorization;
using AutoMapper;

namespace server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : Controller
    {
        private readonly WebAppDbContext WebAppDbContext; 
        private readonly IMapper Mapper;
        public OrderController(WebAppDbContext WebAppDbContext,IMapper Mapper){
            this.WebAppDbContext = WebAppDbContext;
            this.Mapper = Mapper;
        }

        [HttpGet("GetOrders")]
        [Authorize]
        public async Task<ActionResult<List<OrderDTO>>> GetAllOrder(){
            var lst = await WebAppDbContext.Orders.Select(
                e => new OrderDTO
                {
                    OrderId = e.OrderId,
                    UserId = e.UserId,
                    Username = e.Username,
                    ReceiverId = e.ReceiverId,
                    ReceiverUsername = e.ReceiverUsername,
                    PiorityScore = e.PiorityScore,
                    Restaurant = e.Restaurant,
                    Detail = e.Detail,
                    ReceiveLocation = e.ReceiveLocation,
                    IsTaken = e.IsTaken
                }
            ).ToListAsync();

            var sorted = lst.OrderByDescending(e => e.PiorityScore).ToList();

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
        [Authorize]
        public async Task<ActionResult<List<OrderDTO>>> GetMyOrder(string UserId){
            var lst = await WebAppDbContext.Orders.Where(e=> e.UserId == UserId).ToListAsync();
            var myDtos = Mapper.Map<List<OrderDTO>>(lst);
            return myDtos;
        }

        [HttpGet("[action]/{UserId}")]
        [Authorize]
        public async Task<ActionResult<List<OrderDTO>>> GetTheirOrder(string UserId){
            var lst = await WebAppDbContext.Orders.Where(e=> e.UserId != UserId && e.IsTaken!=true).ToListAsync();
            var myDtos = Mapper.Map<List<OrderDTO>>(lst);
            var sorted = myDtos.OrderByDescending(e => e.PiorityScore).ToList();
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
        [Authorize]
        public async Task<ActionResult<List<OrderDTO>>> GetMyTakingOrder(string UserId){
            var lst = await WebAppDbContext.Orders.Where(e=> e.ReceiverId == UserId && e.IsTaken==true).ToListAsync();
            var myDtos = Mapper.Map<List<OrderDTO>>(lst);
            var sorted = myDtos.OrderByDescending(e => e.PiorityScore).ToList();
            if (sorted.Count < 0)
            {
                return NotFound();
            }
            else
            {
                return sorted;
            }
        }

        [HttpPost("AddOrder")]
        [Authorize]
        public async Task<HttpStatusCode> CreateOrder(AddOrderDTO AddOrder){
            string authHeader = Request.Headers["Authorization"];
            string[] lstAuthHeader = authHeader.Split(" "); 
            var token = lstAuthHeader[1];
            JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
            JwtSecurityToken decodedToken = tokenHandler.ReadJwtToken(token);
            var claims = decodedToken.Claims;
            string OwnUserId = claims.FirstOrDefault(c => c.Type == "UserId")?.Value;
            string OwnUsername = claims.FirstOrDefault(c => c.Type == "Username")?.Value;
            int OwnScore = int.Parse(claims.FirstOrDefault(c => c.Type == "Score")?.Value);
            Guid myuuid = Guid.NewGuid();
            string myuuidAsString = myuuid.ToString();
            Console.WriteLine(AddOrder.Detail);
            Console.WriteLine(AddOrder.ReceiveLocation);
            Console.WriteLine(AddOrder.Restaurant);
            try{
                var newOrder = new OrderModel()
                    {
                        OrderId = myuuidAsString,
                        UserId = OwnUserId,
                        Username = OwnUsername,
                        PiorityScore = OwnScore,
                        Restaurant = AddOrder.Restaurant,
                        Detail = AddOrder.Detail,
                        ReceiveLocation = AddOrder.ReceiveLocation,
                        IfDoneScore = AddOrder.IfDoneScore,
                        IsTaken = false
                    };
                WebAppDbContext.Orders.Add(newOrder);
                await WebAppDbContext.SaveChangesAsync();
                return HttpStatusCode.Created;
            }catch{
                return HttpStatusCode.BadRequest;
            }
        }

        [HttpDelete("[action]/{OrderId}")]
        [Authorize]
        public async Task<HttpStatusCode> DeleteOrder(string OrderId){
            try{
                var foundOrder = await WebAppDbContext.Orders.FirstOrDefaultAsync(e=>e.OrderId == OrderId);
                if(foundOrder!=null){
                    WebAppDbContext.Remove(foundOrder);
                    await WebAppDbContext.SaveChangesAsync();
                    return HttpStatusCode.Accepted;
                }
                return HttpStatusCode.BadRequest;
            }catch{
                return HttpStatusCode.InternalServerError;
            }
        }

        [HttpPatch("[action]/{OrderId}")]
        [Authorize]
        public async Task<HttpStatusCode> TakeOrder(string OrderId,TakeOrderDTO takeOrder){
            try{
                var foundOrder = await WebAppDbContext.Orders.FindAsync(OrderId);
                if(foundOrder!=null){
                    foundOrder.ReceiverId = takeOrder.ReceiverId;
                    foundOrder.ReceiverUsername = takeOrder.ReceiverUsername;
                    foundOrder.IsTaken = true;
                    await WebAppDbContext.SaveChangesAsync();
                    return HttpStatusCode.OK;
                }
                return HttpStatusCode.BadRequest;
            }catch{
                return HttpStatusCode.InternalServerError;
            }
        }

        [HttpDelete("[action]/{OrderId}")]
        [Authorize]
        public async Task<HttpStatusCode> SuccessOrder(string OrderId){
            try{
                var foundOrder = await WebAppDbContext.Orders.FirstOrDefaultAsync(e=>e.OrderId == OrderId);
                if(foundOrder!=null){
                    var foundUser = await WebAppDbContext.Users.FirstOrDefaultAsync(e=>e.UserId == foundOrder.ReceiverId);
                    foundUser.Score += foundOrder.IfDoneScore;
                    foundUser.Success = foundUser.Success+1;
                    WebAppDbContext.Remove(foundOrder);
                    await WebAppDbContext.SaveChangesAsync();
                    return HttpStatusCode.Accepted;
                }
                return HttpStatusCode.BadRequest;
            }catch{
                return HttpStatusCode.InternalServerError;
            }
        }

        [HttpDelete("[action]/{OrderId}")]
        [Authorize]
        public async Task<HttpStatusCode> DenyOrder(string OrderId){
            try{
                var foundOrder = await WebAppDbContext.Orders.FirstOrDefaultAsync(e=>e.OrderId == OrderId);
                if(foundOrder!=null){
                    var foundUser = await WebAppDbContext.Users.FirstOrDefaultAsync(e=>e.UserId == foundOrder.ReceiverId);
                    foundUser.Failed = foundUser.Failed+1;
                    WebAppDbContext.Remove(foundOrder);
                    await WebAppDbContext.SaveChangesAsync();
                    return HttpStatusCode.Accepted;
                }
                return HttpStatusCode.BadRequest;
            }catch{
                return HttpStatusCode.InternalServerError;
            }
        }
    }
}