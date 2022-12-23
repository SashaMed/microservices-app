using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProductAPI.Models.Dto;
using ProductAPI.Repository;

namespace ProductAPI.Controllers
{
	[Route("api/products")]
	[ApiController]
	public class ProductAPIController : ControllerBase
	{
		protected ResponceDto _responce;
		private IProductRepository _repository;

		public ProductAPIController(IProductRepository repository)
		{
			_repository= repository;
			_responce = new ResponceDto();
		}

		[Authorize]
		[HttpGet]
		public async Task<object> GetProducts()
		{
			var productDtos = await _repository.GetProducts();
			_responce.Result = productDtos;
			return _responce;
		}

        [Authorize]
        [HttpGet]
		[Route("{id}")]
		public async Task<object> GetProductById(int id)
		{
			var productDto = await _repository.GetProductById(id);
			_responce.Result = productDto;
			return _responce;
		}

        [Authorize]
        [HttpPost]
		public async Task<object> PostProduct([FromBody] ProductDto productDto)
		{
			var model = await _repository.CreateUpdateProduct(productDto);
			_responce.Result = model;
			return _responce;
		}

		[Authorize]
		[HttpPut]
		public async Task<object> CreateUpdateProduct([FromBody] ProductDto productDto)
		{
			var model = await _repository.CreateUpdateProduct(productDto);
			_responce.Result = model;
			return _responce;
		}

        [Authorize(Roles = "Admin")]
        [HttpDelete]
		[Route("{id}")]
		public async Task<object> DeleteProduct(int id)
		{
			var result = await _repository.DeleteProduct(id);
			_responce.IsSucces = result;
			return _responce;
		}
	}
}
