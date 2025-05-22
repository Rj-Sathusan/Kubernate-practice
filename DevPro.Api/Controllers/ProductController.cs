using DevPro.Application;
using DevPro.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace DevPro.Api.Controllers
{
    [ApiController]
    [Route("api/productstest")]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;
        private readonly ILogger<ProductController> _logger;

        public ProductController(ILogger<ProductController> logger, IProductService productService)
        {
            _logger = logger;
            _productService = productService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var response = await _productService.GetAllAsync();
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Product product)
        {
            var response = await _productService.CreateAsync(product);
            if (response.Status)
                return Ok(response);
            else
                return BadRequest(response);
        }
    }
}
