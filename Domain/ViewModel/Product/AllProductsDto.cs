namespace Domain.ViewModel.Product
{
    public class AllProductsDto
    {
        public Guid Id { get; set; }
        public required string Name { get; set; }
        public List<AllProductsColorImageDto> ColorsAImages { get; set; }
        public required string CategoryName { get; set; }
        public required string Brand { get; set; }
        public int Sale { get; set; }
        public decimal Price { get; set; }
    }
}
