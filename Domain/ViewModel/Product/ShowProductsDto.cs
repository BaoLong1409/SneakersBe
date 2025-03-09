namespace Domain.ViewModel.Product
{
    public class ShowProductsDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string ThumbnailImage { get; set; }
        public string Colors { get; set; }
        public int Sale { get; set; }
        public int Rating { get; set; }
        public decimal Price { get; set; }

    }
}
