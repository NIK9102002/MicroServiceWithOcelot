using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProductAPI.Application.DTOs;
using ProductAPI.Application.Interfaces;
using ProductAPI.Domain.Entities;

namespace ProductAPI.Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [AllowAnonymous]
    public class ProductsController : ControllerBase
    {
        private readonly IProduct _productInterface;
        private readonly IMapper _mapper;
        public ProductsController(IProduct productInterface, IMapper mapper)
        {
            _productInterface = productInterface;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProductDTO>>> GetProducts()
        {
            var products = await _productInterface.GetAllAsync();
            if (!products.Any())
            {
                return NotFound("No products found.");
            }

            var list = _mapper.Map<List<ProductDTO>>(products);

            return list.Any() ? Ok(list) : NotFound("No products found.");
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<ProductDTO>> GetProduct(int id)
        {
            var product = await _productInterface.GetByIdAsync(id);
            if (product is null)
            {
                return NotFound("Product not found.");
            }

            var dto = _mapper.Map<ProductDTO>(product);
            return Ok(dto);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<ProductDTO>> CreateProduct(ProductDTO productDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var product = _mapper.Map<Product>(productDto);
            var response = await _productInterface.CreateAsync(product);
            return response.Flag is true ? Ok(response) : BadRequest(response);
        }

        [HttpPut]
        [Authorize(Roles = "Admin")]

        public async Task<ActionResult<ProductDTO>> UpdateProduct(ProductDTO productDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var productToUpdate = _mapper.Map<Product>(productDto);
            var response = await _productInterface.UpdateAsync(productToUpdate);

            return response.Flag is true ? Ok(response) : BadRequest(response);
        }

        [HttpDelete]
        [Authorize(Roles = "Admin")]

        public async Task<ActionResult> DeleteProduct(int id)
        {
            var response = await _productInterface.DeleteAsync(id);
            return response.Flag is true ? Ok(response) : BadRequest(response);
        }
    }
}
