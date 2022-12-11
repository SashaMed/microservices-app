using FrontEnd.Models;
using FrontEnd.Models.Dto;
using FrontEnd.Services.IServices;

namespace FrontEnd.Services
{
	public class ProductService : BaseService, IProductService
	{
		private readonly IHttpClientFactory _clientFactory;
		public ProductService(IHttpClientFactory httpClient) : base(httpClient)
		{
			_clientFactory = httpClient;
		}

		public ResponceDto ResponceModel { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

		public async Task<T> CreateProductsAsync<T>(ProductDto productDto)
		{
			return await SendAsync<T>(new ApiRequest
			{
				ApiType = StaticData.ApiType.POST,
				Data = productDto,
				Url = StaticData.ProductAPIBase + "/api/products",
				AccessToken = ""
			});
		}

		public async Task<T> DeleteProductsAsync<T>(int id)
		{
			return await SendAsync<T>(new ApiRequest
			{
				ApiType = StaticData.ApiType.DELETE,
				Url = StaticData.ProductAPIBase + "/api/products/" + id.ToString(),
				AccessToken = ""
			});
		}


		public async Task<T> GetAllProductsAsync<T>()
		{
			return await SendAsync<T>(new ApiRequest
			{
				ApiType = StaticData.ApiType.GET,
				Url = StaticData.ProductAPIBase + "/api/products",
				AccessToken = ""
			});
		}

		public async Task<T> GetProductByIdAsync<T>(int id)
		{
			return await SendAsync<T>(new ApiRequest
			{
				ApiType = StaticData.ApiType.GET,
				Url = StaticData.ProductAPIBase + "/api/products/" + id.ToString(),
				AccessToken = ""
			});
		}


		public async Task<T> UpdateProductsAsync<T>(ProductDto productDto)
		{
			return await SendAsync<T>(new ApiRequest
			{
				ApiType = StaticData.ApiType.PUT,
				Data = productDto,
				Url = StaticData.ProductAPIBase + "/api/products",
				AccessToken = ""
			});
		}
	}
}
