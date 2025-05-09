﻿namespace Domain.ViewModel.Product
{
    public class FeatureProductModel
    {
        public required string ImageUrl { get; set; }
        public required string ProductName { get; set; }
        public required decimal Price { get; set; }
        public required string LeftColor { get; set; }
        public required string MiddleColor { get; set; }
        public required string RightColor { get; set; }
    }
}
