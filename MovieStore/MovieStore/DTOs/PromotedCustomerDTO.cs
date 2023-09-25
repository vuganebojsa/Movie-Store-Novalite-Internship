namespace MovieStore.Api.DTOs
{
    public class PromotedCustomerDTO
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public decimal MoneySpent { get; set; } = 0;
    }
}
