namespace Domain.ViewModel.Product
{
    public class DetailProductDto
    {
        public Guid Id { get; set; }
        public required string Name { get; set; }
        public string? Description { get; set; }
        public decimal Price { get; set; }
        public int Sale { get; set; }
        public DateTime CreatedAt { get; set; }
        public List<ProductImageDto>? ProductImages { get; set; }
    }
}
