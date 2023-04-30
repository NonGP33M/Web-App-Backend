namespace server.DTO.User
{
    public class UserDTO
    {
        public string? UserId { get; set; } = string.Empty;

        public string? Username { get; set; } = string.Empty;

        public string? Password { get; set; } = string.Empty;

        public string? UserImg { get; set; } = string.Empty;


        public string? FirstName { get; set; } = string.Empty;


        public string? LastName { get; set; } = string.Empty;

        public string? Tel { get; set; } = string.Empty;

        public int Score { get; set; }


        public int Success { get; set; }

        public int Failed { get; set; }
    }
}