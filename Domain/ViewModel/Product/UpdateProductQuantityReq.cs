namespace Domain.ViewModel.Product
{
    public class UpdateProductQuantityReq
    {
        public Guid ProductId { get; set; }
        public Guid ColorId { get; set; }
        public Guid SizeId { get; set; }
        public int Quantity { get; set; }
    }
}
