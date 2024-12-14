using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Talabat.APIs.DTOs;
using Talabat.APIs.Errors;
using Talabat.APIs.Helpers;
using Talabat.Core;
using Talabat.Core.Entities;
using Talabat.Core.Specifications;

namespace Talabat.APIs.Controllers;
public class ProductsController : ApiBaseController
{
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;

    public ProductsController(IMapper mapper , IUnitOfWork unitOfWork )
    {
        _mapper = mapper;
        this._unitOfWork = unitOfWork;
    }

    // Get ALl Products [EndPoint]
    [CachedAttribute(300)]
    [HttpGet]
    public async Task<ActionResult<Pagination<ProductToReturnDto>>> GetProducts([FromQuery]ProductSpecParams Params)
    {
        var Spec = new ProductWithBrandAndTypeSpecification(Params);
        var Products = await _unitOfWork.Repository<Product>().GetAllWithSpecAsync(Spec);
        var MappedProducts = _mapper.Map< IReadOnlyList<Product> , IReadOnlyList<ProductToReturnDto> >(Products);

        //OkObjectResult Result = new OkObjectResult(Products);        
        //return Ok(Result);

        //var ReturnedObject = new Pagination<ProductToReturnDto>()
        //{
        //    PageIndex = Params.PageIndex,
        //    PageSize = Params.PageSize,
        //    Data = MappedProducts
        //};
        var CountSpec = new ProductWithFilterationForCountAsync(Params);
        var Count = await _unitOfWork.Repository<Product>().GetCountWithSpecAsync(CountSpec);
        return Ok(new Pagination<ProductToReturnDto>(Params.PageIndex, Params.PageSize, MappedProducts, Count)); // using helper method of OkObjectResult
    } 

    //Get Product by Id[EndPoint]
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(ProductToReturnDto), 200)]
    [ProducesResponseType(typeof(ApiResponse) , StatusCodes.Status404NotFound)]
    public async Task<ActionResult<Product>> GetProductsById(int id)
    {
        var Spec = new ProductWithBrandAndTypeSpecification(id);
        var Product = await _unitOfWork.Repository<Product>().GetEntityWithSpecAsync(Spec);
        if (Product == null) return NotFound(new ApiResponse(404));
        var MappedProduct = _mapper.Map<Product, ProductToReturnDto>(Product);
        return Ok(MappedProduct);
    }

    [HttpGet("types")]
    public async Task<ActionResult<IReadOnlyList<ProductType>>> GetTypes()
    {
        var types = await _unitOfWork.Repository<ProductType>().GetAllAsync();
        return Ok(types);
    }

    [HttpGet("brands")]
    public async Task<ActionResult<IReadOnlyList<ProductBrand>>> GetBrands()
    {
        var brands = await _unitOfWork.Repository<ProductBrand>().GetAllAsync();
        return Ok(brands);
    }
}
