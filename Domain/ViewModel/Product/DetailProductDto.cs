using Domain.ViewModel.Category;

namespace Domain.ViewModel.Product
{
    public class DetailProductDto
    {
        public Guid ProductId { get; set; }
        public required string ProductName { get; set; }
        public Guid ColorId { get; set; }
        public required string ColorName { get; set; }
        public required List<CategoryDto> Categories { get; set; }
        //public string? Description { get; set; }
        public decimal Price { get; set; }
        public int Sale { get; set; }
        public DateTime CreatedAt { get; set; }
        public List<ProductImageDto>? ProductImages { get; set; }
    }
}
