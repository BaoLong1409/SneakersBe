using Azure;
using Dapper;
using DataAccess.DbContext;
using Domain.ViewModel.Product;
using MediatR;
using Sneakers.Features.Queries.Products;
using System.Text.Json;

namespace Sneakers.Handler.QueriesHandler.ProductsHandler
{
    public class GetRecommendProductsHandler : IRequestHandler<GetRecommendProducts, IEnumerable<ShowProductsDto>>
    {
        private readonly SneakersDapperContext _context;
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _config;
        public GetRecommendProductsHandler(SneakersDapperContext context, HttpClient httpClient, IConfiguration config)
        {
            _context = context;
            _httpClient = httpClient;
            _config = config;
        }
        public async Task<IEnumerable<ShowProductsDto>> Handle(GetRecommendProducts request, CancellationToken cancellationToken)
        {
            string apiUrl = $"{_config["PythonApi:Url"]}/nearestUser?userId={request.UserId}";
            HttpResponseMessage respone = await _httpClient.GetAsync(apiUrl, cancellationToken);
            if (!respone.IsSuccessStatusCode)
            {
                throw new Exception($"API call failed: {respone.StatusCode}");
            }
            string responseResult = await respone.Content.ReadAsStringAsync(cancellationToken);
            var result = JsonSerializer.Deserialize<RecommendationUsers>(responseResult);

            if (result == null)
            {
                return new List<ShowProductsDto>();
            }

            return await GetRecommendedProductsByUsers(result);
        }

        private async Task<IEnumerable<ShowProductsDto>> GetRecommendedProductsByUsers(RecommendationUsers nearestUsers)
        {
            var query = @"
            WITH RankedProducts AS (
                SELECT p.*, i.imageUrl AS thumbnailImage, inter.UserId, ROW_NUMBER() OVER (PARTITION BY inter.UserId ORDER BY inter.TimeStamp) AS RowNum 
                FROM Product p 
                INNER JOIN Interaction inter ON p.Id = inter.ProductId 
                INNER JOIN ProductImage i ON p.Id = i.ProductId 
                WHERE inter.UserId IN (SELECT CAST(value AS UNIQUEIDENTIFIER) FROM STRING_SPLIT(@userIds, ',')) AND i.IsThumbnail = 1 
            ) 
            SELECT DISTINCT rp.Id, rp.ProductName, rp.ThumbnailImage FROM RankedProducts rp WHERE RowNum <= (30 / (SELECT COUNT (DISTINCT inter.UserId) FROM Interaction inter WHERE UserId IN (SELECT CAST(value AS UNIQUEIDENTIFIER) FROM STRING_SPLIT(@userIds , ','))))";

            using (var connection = _context.CreateConnection())
            {
                var userIdString = string.Join(",", nearestUsers.RecommendUsers);
                var products = await connection.QueryAsync<ShowProductsDto>(query, new { userIds = userIdString});
                return products.ToList();
            }
        }

        private class RecommendationUsers
        {
            public List<string> RecommendUsers { get; set; }
        }
    }
}
