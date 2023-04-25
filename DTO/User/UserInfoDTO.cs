namespace server.DTO.User
{
    public class UserInfoDTO
    {
        public string? Username { get; set; }
        public int Score { get; set; }
        public string? UserImg { get; set; }
        public string? FirstName {get;set;}
        public string? LastName {get;set;}
        public int Success {get;set;}
        public int Failed {get;set;}

    }
}