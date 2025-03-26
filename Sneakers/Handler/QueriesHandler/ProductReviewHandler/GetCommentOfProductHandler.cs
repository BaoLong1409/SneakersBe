using Dapper;
using DataAccess.DbContext;
using Domain.ViewModel.ProductReview;
using MediatR;
using Sneakers.Features.Queries.ProductReview;

namespace Sneakers.Handler.QueriesHandler.ProductReviewHandler
{
    public class GetCommentOfProductHandler : IRequestHandler<GetCommentOfProduct, IEnumerable<ProductReviewDto>>
    {
        private readonly SneakersDapperContext _context;
        public GetCommentOfProductHandler(SneakersDapperContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<ProductReviewDto>> Handle(GetCommentOfProduct request, CancellationToken cancellationToken)
        {
            var query = @"SELECT pr.Id, pr.CommentContent, pr.Quality, pr.UpdatedAt, u.FirstName, u.LastName, pri.ImageUrl FROM [ProductReview] pr
                            JOIN Color c ON c.Name = @ColorName
                            JOIN OrderDetail od ON od.Id = pr.OrderDetailId AND od.Reviewed = 1 AND od.ColorId = c.Id
                            JOIN ProductReviewImage pri ON pri.ProductReviewId = pr.Id
                            JOIN Product p ON p.Id = od.ProductId
                            JOIN AspNetUsers u ON u.Id = pr.UserId
                            WHERE p.Id = @ProductId";
            var commentDic = new Dictionary<Guid, ProductReviewDto>();
            using (var connection = _context.CreateConnection())
            {
                var comments = await connection.QueryAsync<ProductReviewDto, string, ProductReviewDto>(query, (comment, imageUrl) =>
                {
                    if (!commentDic.TryGetValue(comment.Id, out var commentEntry))
                    {
                        commentEntry = comment;
                        commentEntry.ImageUrl = new List<string>();
                        commentDic.Add(comment.Id, commentEntry);
                    }

                    if (!string.IsNullOrEmpty(imageUrl))
                    {
                        commentEntry.ImageUrl.Add(imageUrl);

                    }
                    return commentEntry;
                },
                splitOn: "ImageUrl",
                param: new { ProductId =  request.ProductId, ColorName = request.ColorName }
                );

                return commentDic.Values.ToList();
            }
        }
    }
}
