using FrontEnd.Models;
using FrontEnd.Services.IServices;
using Newtonsoft.Json.Linq;

namespace FrontEnd.Services
{
    public class CouponService : BaseService, ICouponService
    {
        private readonly IHttpClientFactory _clientFactory;

        public CouponService(IHttpClientFactory httpClient) : base(httpClient)
        {
            _clientFactory = httpClient;
        }

        public async Task<T> GetCoupon<T>(string coupon, string token = null)
        {
            return await SendAsync<T>(new ApiRequest
            {
                ApiType = StaticData.ApiType.GET,
                Url = StaticData.CouponAPIBase + "/api/coupon/" + coupon,
                AccessToken = token
            });
        }
    }
}
