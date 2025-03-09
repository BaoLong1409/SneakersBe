namespace Domain.ViewModel.Product
{
    public class AllProductsColorImageDto
    {
        public Guid ColorId { get; set; }
        public Guid ImageId { get; set; }
        public required string ColorName { get; set; }
        public required string ThumbnailUrl { get; set; }
    }
}
