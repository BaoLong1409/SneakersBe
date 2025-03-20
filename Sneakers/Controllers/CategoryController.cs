using Microsoft.AspNetCore.Mvc;
using Sneakers.Features.Queries.FeatureProducts;
using Sneakers.Services.CategoryService;

namespace Sneakers.Controllers
{
    [Route("api/v1")]
    [ApiController]
    public class CategoryController : Controller
    {
        private readonly CategoryService _categoryService;
        public CategoryController(CategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        [HttpGet]
        [Route("category/getAll")]
        public IActionResult GetAllCategory()
        {
            return Ok(_categoryService.GetAllCategory());
        }
    }
}
