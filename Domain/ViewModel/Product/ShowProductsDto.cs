namespace Domain.ViewModel.Product
{
    public class ShowProductsDto
    {
        public Guid ProductId { get; set; }
        public string ProductName { get; set; }
        public string ThumbnailImage { get; set; }
        public string Colors { get; set; }
        public int Sale { get; set; }
        public decimal Price { get; set; }

    }
}
