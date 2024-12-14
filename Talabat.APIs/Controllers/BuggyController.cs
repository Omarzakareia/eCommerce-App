using Microsoft.AspNetCore.Mvc;
using Talabat.APIs.Errors;
using Talabat.Repository.Data;

namespace Talabat.APIs.Controllers;
[ApiExplorerSettings(IgnoreApi = true)]
public class BuggyController : ApiBaseController
{
    private readonly StoreContext _dbContext;

    public BuggyController(StoreContext dbContext)
    {
        _dbContext = dbContext;
    }
    // BaseUrl/api/Buggy/NotFound
    [HttpGet("NotFound")]
    public ActionResult GetNotFoundRequest()
    {
        var Product = _dbContext.Products.Find(100);
        if (Product == null)
            return NotFound(new ApiResponse(404));
        else
            return Ok(Product);
    }

    [HttpGet("ServerError")]
    public ActionResult GetServerError()
    {
        var Product = _dbContext.Products.Find(100);
        var ProductToString = Product.ToString();
        return Ok(ProductToString);
    }

    [HttpGet("BadRequest")]
    public ActionResult GetBadRequest()
    {
        return BadRequest();
    }

    [HttpGet("BadRequest/{id}")]
    public ActionResult GetBadRequest(int id)
    {
        return Ok();
    }


}
