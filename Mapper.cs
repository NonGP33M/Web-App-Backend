using AutoMapper;
using server.model;
using server.DTO.Order;
using server.DTO.User;

namespace server
{
    public class Mapper : Profile
    {
        public Mapper()
        {
            CreateMap<OrderModel,OrderDTO>();
            CreateMap<UserModel,UserDTO>();
        }
    }
}