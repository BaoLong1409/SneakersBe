namespace Domain.ViewModel.Product
{
    public class AllProductsDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public List<AllProductsColorImageDto> ColorsAImages { get; set; }
        public int Sale { get; set; }
        public int Rating { get; set; }
        public decimal Price { get; set; }
    }
}
