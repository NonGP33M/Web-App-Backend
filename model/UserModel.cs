using System.ComponentModel.DataAnnotations;

namespace server.model
{
    public class UserModel
    {
        [Key]
        [StringLength(45)]
        public string? UserId{get;set;}= string.Empty;

        [StringLength(45)]
        public string? Username {get;set;}= string.Empty;

        [StringLength(500)]
        public string? Password {get;set;}= string.Empty;

        [StringLength(500)]
        public string? UserImg {get;set;}= string.Empty;

        [StringLength(45)]
        public string? FistName {get;set;}= string.Empty;

        [StringLength(45)]
        public string? LastName{get;set;}= string.Empty;

        public int Score {get;set;}

        public int Success {get;set;}

        public int Failed {get;set;}
    }
}