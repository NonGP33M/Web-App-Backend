namespace server.DTO.Order
{
    public class AddOrderDTO
    {
        public string? Restaurant { get; set; }

        public string? Detail { get; set; } 

        public string? ReceiveLocation { get; set; }

        public int IfDoneScore {get;set;}
    }
}