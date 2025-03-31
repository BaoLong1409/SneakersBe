namespace Domain.ViewModel.Product
{
    public class AllProductsDto
    {
        public Guid ProductId { get; set; }
        public required string ProductName { get; set; }
        public List<AllProductsColorImageDto> ColorsAImages { get; set; }
        public required string CategoryName { get; set; }
        public required string Brand { get; set; }
        public int Sale { get; set; }
        public decimal Price { get; set; }
    }
}
