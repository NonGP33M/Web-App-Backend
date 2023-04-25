using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace server.DTO.Auth
{
    public class LoginDTO
    {
        public string? Username { get; set; }
        public string? Password { get; set; }
    }
}