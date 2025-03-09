namespace Domain.ViewModel.Product
{
    public class ImageProductDto
    {
        public Guid ImageId { get; set; }
        public string ImageUrl { get; set; }
        public int IsThumbnail { get; set; }
        public Guid ProductId { get; set; }
        public Guid ColorId { get; set; }
        public string ColorName { get; set; }
    }
}
