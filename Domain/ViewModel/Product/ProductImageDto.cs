namespace Domain.ViewModel.Product
{
    public class ProductImageDto
    {
        public Guid ProductImageId { get; set; }
        public required string ImageUrl { get; set; }
        public int IsThumbnail { get; set; }
        public Guid ProductId { get; set; }
        public Guid ColorId { get; set; }
    }
}
