using Microservice3.API.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Microservice3.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController(AppDbContext context) : ControllerBase
    {
        //get
        [HttpGet]
        public async Task<ActionResult<List<Product>>> Get()
        {
            return await context.Products.ToListAsync();
        }
    }
}
