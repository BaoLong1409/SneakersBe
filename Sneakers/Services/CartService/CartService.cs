﻿using AutoMapper;
using Domain.Entities;
using Domain.Enum;
using Domain.Interfaces;
using Domain.ViewModel;

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

        public async Task<IEnumerable<ProductInCartDto>> GetProductInCartsAsync(Guid userId)
        {
            var cart = await _unitOfWork.Cart.GetFirstOrDefaultAsync(x => x.UserId == userId);

            if (cart == null) return new List<ProductInCartDto>();

            return await _unitOfWork.ProductCart.GetProductInCartsAsync(cart.Id);
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

            if (cart == null) return EnumProductCart.CartNotFound;

            var existingProduct = await _unitOfWork.ProductCart.GetFirstOrDefaultAsync(pc =>
            pc.CartId == cart.Id &&
            pc.ProductId == product.ProductId &&
            pc.SizeId == product.SizeId &&
            pc.ColorId == product.ColorId
            );

            if (existingProduct == null) {
                return EnumProductCart.ProductNotFound;
            }

            _unitOfWork.ProductCart.Remove(existingProduct);
            _unitOfWork.Complete();

            return EnumProductCart.Success;
        }
    }
}
