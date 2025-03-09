namespace Domain.ViewModel.Product
{
    public class ProductImageDto
    {
        public required string ImageUrl { get; set; }
        public int IsThumbnail { get; set; }
        public Guid ProductId { get; set; }
    }
}
