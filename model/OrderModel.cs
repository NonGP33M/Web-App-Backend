using System.ComponentModel.DataAnnotations;

namespace server.model
{
    public class OrderModel
    {
        [Key]
        [StringLength(45)]
        public string? OrderId{get;set;}= string.Empty;
        
        [StringLength(45)]
        public string? UserId{get;set;}= string.Empty;

        [StringLength(45)]
        public string? Username {get;set;}= string.Empty;

        [StringLength(45)]
        public string? ReceiverId {get;set;}= string.Empty;

        [StringLength(45)]
        public string? ReceiverUsername {get;set;}= string.Empty;

        public int PiorityScore {get;set;}

        [StringLength(45)]
        public string? Restaurant {get;set;}= string.Empty;

        [StringLength(500)]
        public string? Detail {get;set;}= string.Empty;

        [StringLength(45)]
        public string? ReceiveLocation {get;set;}= string.Empty;

        public int IfDoneScore {get;set;}

        public bool? IsTaken {get;set;}
    }
}