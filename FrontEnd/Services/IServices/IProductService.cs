using FrontEnd.Models.Dto;
using Newtonsoft.Json.Linq;

namespace FrontEnd.Services.IServices
{
	public interface IProductService : IBaseService
	{
		Task<T> GetAllProductsAsync<T>(string token);
		Task<T> GetProductByIdAsync<T>(int id, string token);
		Task<T> CreateProductsAsync<T>(ProductDto productDto,string token);
		Task<T> UpdateProductsAsync<T>(ProductDto productDto, string token);
		Task<T> DeleteProductsAsync<T>(int id, string token);
	}
}
