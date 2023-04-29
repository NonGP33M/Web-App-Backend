namespace server.DTO.User
{
    public class UpdateUserDTO
    {
        public IFormFile? Image{get;set;}
        public string? FirstName{get;set;}
        public string? LastName{get;set;}
        public string? Tel {get;set;}
    }
}