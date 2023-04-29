namespace server.DTO.Order
{
    public class OrderDTO
    {
        public string? OrderId{get;set;}= string.Empty;
        
        public string? UserId{get;set;}= string.Empty;

        public string? Username {get;set;}= string.Empty;

        public string? UserTel {get;set;}= string.Empty;

        public string? ReceiverTel {get;set;}= string.Empty;

        public string? ReceiverId {get;set;}= string.Empty;

        public string? ReceiverUsername {get;set;}= string.Empty;

        public int PiorityScore {get;set;}

        public string? Restaurant {get;set;}= string.Empty;

        public string? Detail {get;set;}= string.Empty;

        public string? ReceiveLocation {get;set;}= string.Empty;

        public int IfDoneScore {get;set;}

        public bool? IsTaken {get;set;}
    }
}