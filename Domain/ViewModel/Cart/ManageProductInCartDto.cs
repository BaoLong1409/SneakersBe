namespace Domain.ViewModel.Cart
{
    public class ManageProductInCartDto
    {
        public Guid ProductId { get; set; }
        public Guid? CartId { get; set; }
        public Guid SizeId { get; set; }
        public Guid ColorId { get; set; }
        public int Quantity { get; set; }
    }
}
