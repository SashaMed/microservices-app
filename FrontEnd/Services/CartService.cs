using FrontEnd.Models;
using FrontEnd.Models.Dto;
using FrontEnd.Services.IServices;

namespace FrontEnd.Services
{
    public class CartService : BaseService, ICartService
    {
        private readonly IHttpClientFactory _clientFactory;
        
        public CartService(IHttpClientFactory httpClient) : base(httpClient)
        {
            _clientFactory = httpClient;
        }



        public async Task<T> AddToCartAsync<T>(CartDto cartDto, string token = null)
        {
            return await SendAsync<T>(new ApiRequest
            {
                ApiType = StaticData.ApiType.POST,
                Data = cartDto,
                Url = StaticData.ShoppingCartAPIBase + "/api/carts",
                AccessToken = token
            }) ;
        }

        public async Task<T> ApplyCoupon<T>(CartDto cartDto, string token = null)
        {
            return await SendAsync<T>(new ApiRequest
            {
                ApiType = StaticData.ApiType.POST,
                Data = cartDto,
                Url = StaticData.ShoppingCartAPIBase + "/api/carts/apply-coupon",
                AccessToken = token
            });
        }

        public async Task<T> Checkout<T>(CartHeaderDto cartHeader, string token = null)
        {
            return await SendAsync<T>(new ApiRequest
            {
                ApiType = StaticData.ApiType.POST,
                Data = cartHeader,
                Url = StaticData.ShoppingCartAPIBase + "/api/carts/checkout",
                AccessToken = token
            });
        }

        public async Task<T> GetCartbyUserAsync<T>(string userId, string token = null)
        {
            return await SendAsync<T>(new ApiRequest
            {
                ApiType = StaticData.ApiType.GET,
                Url = StaticData.ShoppingCartAPIBase + "/api/carts/" + userId,
                AccessToken = token
            });
        }

        public async Task<T> RemoveCoupon<T>(string userId, string token = null)
        {
            return await SendAsync<T>(new ApiRequest
            {
                ApiType = StaticData.ApiType.POST,
                Url = StaticData.ShoppingCartAPIBase + $"/api/carts/remove-coupon/{userId}",
                AccessToken = token
            });
        }

        public async Task<T> RemoveFromCartAsync<T>(int cartId, string token = null)
        {
            return await SendAsync<T>(new ApiRequest
            {
                ApiType = StaticData.ApiType.POST,
                //Data = cartId,
                Url = StaticData.ShoppingCartAPIBase + $"/api/carts/{cartId.ToString()}",
                AccessToken = token
            });
        }

        public async Task<T> UpdateCartAsync<T>(CartDto cartDto, string token = null)
        {
            return await SendAsync<T>(new ApiRequest
            {
                ApiType = StaticData.ApiType.PUT,
                Data = cartDto,
                Url = StaticData.ShoppingCartAPIBase + "/api/carts",
                AccessToken = token
            });
        }
    }
}
