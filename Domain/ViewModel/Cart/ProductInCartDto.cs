namespace Domain.ViewModel.Cart
{
    public class ProductInCartDto
    {
        public Guid ProductId { get; set; }
        public string ProductName { get; set; }
        public decimal Price { get; set; }
        public string ImageUrl { get; set; }
        public Guid ColorId { get; set; }
        public string ColorName { get; set; }
        public Guid SizeId { get; set; }
        public float SizeNumber { get; set; }
        public int Quantity { get; set; }

    }
}
