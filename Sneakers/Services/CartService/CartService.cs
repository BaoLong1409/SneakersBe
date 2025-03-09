using AutoMapper;
using Domain.Entities;
using Domain.Enum;
using Domain.Interfaces;
using Domain.ViewModel.Cart;
using Microsoft.AspNetCore.Mvc;

namespace Sneakers.Services.CartService
{
    public class CartService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public CartService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        private Cart CreateNewCart(Cart cart, Guid userId)
        {
            _unitOfWork.Cart.Add(cart);
            return cart;
        }

        public async Task<EnumProductCart> AddProductToCart( ManageProductInCartDto product, Guid userId)
        {
            var cart = await _unitOfWork.Cart.GetFirstOrDefaultAsync(c => c.UserId == userId);
            if (cart == null)
            {
                cart = CreateNewCart(new Cart
                {
                    TotalProducts = 1,
                    UserId = userId
                }, userId);
            }

            var productStockQuantity = await _unitOfWork.ProductQuantity
                .GetFirstOrDefaultAsync(c =>
                c.ProductId == product.ProductId &&
                c.SizeId == product.SizeId &&
                c.ColorId == product.ColorId
                );
            if (productStockQuantity == null)
            {
                return EnumProductCart.ProductNotFound;
            }

            if (productStockQuantity.StockQuantity < product.Quantity)
            {
                return EnumProductCart.NotEnoughInStock;
            }

            var productInCart = await GetProductInCartAsync(cart.Id, product);
            var productToAddCart = _mapper.Map<ProductCart>(product);
            productToAddCart.CartId = cart.Id;

            if (productInCart == null) {
                _unitOfWork.ProductCart.Add(productToAddCart);
                cart.TotalProducts = cart.TotalProducts + 1;
                _unitOfWork.Complete();
                return EnumProductCart.Success;
            }
            if ((productInCart.Quantity + product.Quantity) > productStockQuantity.StockQuantity)
            {
                return EnumProductCart.NotEnoughInStock;
            }

            productInCart.Quantity += product.Quantity;
            _unitOfWork.Complete();
            return EnumProductCart.Success;
        }

        private async Task<ProductCart> GetProductInCartAsync(Guid cartId, ManageProductInCartDto product)
        {
            var productInCart = await _unitOfWork.ProductCart.GetFirstOrDefaultAsync(x => x.CartId == cartId && x.ProductId == product.ProductId && x.SizeId == product.SizeId && x.ColorId == product.ColorId);

            if (productInCart == null) return null;

            return productInCart;
        }

        public async Task<IEnumerable<ProductInCartDto>> GetProductsInCartAsync(Guid userId)
        {
            var cart = await _unitOfWork.Cart.GetFirstOrDefaultAsync(x => x.UserId == userId);

            if (cart == null) return new List<ProductInCartDto>();
            var products = await _unitOfWork.ProductCart.GetProductInCartsAsync(cart.Id);

            return products.OrderBy(x => x.Name).ToList();
        }

        public async Task<EnumProductCart> UpdateProductInCart(ManageProductInCartDto product, Guid userId)
        {
            var cart = await _unitOfWork.Cart.GetFirstOrDefaultAsync(x => x.UserId == userId);

            if (cart == null) return EnumProductCart.CartNotFound;

            var productStockQuantity = await _unitOfWork.ProductQuantity
                .GetFirstOrDefaultAsync(c =>
                c.ProductId == product.ProductId &&
                c.SizeId == product.SizeId &&
                c.ColorId == product.ColorId
                );

            if (productStockQuantity == null)
            {
                return EnumProductCart.ProductNotFound;
            }

            if (productStockQuantity.StockQuantity < product.Quantity)
            {
                return EnumProductCart.NotEnoughInStock;
            }


            var productToAddCart = _mapper.Map<ProductCart>(product);
            productToAddCart.CartId = cart.Id;

            var existingProduct = await _unitOfWork.ProductCart.GetFirstOrDefaultAsync(pc => 
            pc.CartId == cart.Id && 
            pc.ProductId == product.ProductId && 
            pc.SizeId == product.SizeId && 
            pc.ColorId == product.ColorId
            );

            if (existingProduct == null)
            {
                _unitOfWork.ProductCart.Add(productToAddCart);
                cart.TotalProducts = cart.TotalProducts + 1;
                _unitOfWork.Complete();
                return EnumProductCart.NotExist;
            }

            if (product.Quantity == 0)
            {
                _unitOfWork.ProductCart.Remove(existingProduct);
                _unitOfWork.Complete();
            }

            existingProduct.Quantity = product.Quantity;
            _unitOfWork.Complete();
            return EnumProductCart.Success;
        }

        public async Task<EnumProductCart> DeleteProductInCart(ManageProductInCartDto product, Guid userId)
        {
            var cart = await _unitOfWork.Cart.GetFirstOrDefaultAsync(x => x.UserId == userId);

            if (cart == null)
            {
                cart = CreateNewCart(new Cart
                {
                    TotalProducts = 1,
                    UserId = userId
                }, userId);
            }

            var existingProduct = await _unitOfWork.ProductCart.GetFirstOrDefaultAsync(pc =>
            pc.CartId == cart.Id &&
            pc.ProductId == product.ProductId &&
            pc.SizeId == product.SizeId &&
            pc.ColorId == product.ColorId
            );

            if (existingProduct == null) {
                return EnumProductCart.ProductNotFound;
            }
            cart.TotalProducts = cart.TotalProducts - 1;
            _unitOfWork.ProductCart.Remove(existingProduct);
            _unitOfWork.Complete();

            return EnumProductCart.Success;
        }
    }
}
