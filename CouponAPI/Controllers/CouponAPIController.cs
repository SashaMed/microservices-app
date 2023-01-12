using CouponAPI.Models.Dtos;
using CouponAPI.Repository.Abstract;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CouponAPI.Controllers
{
    [Route("api/coupon")]
    [ApiController]
    public class CouponAPIController : ControllerBase
    {
        private readonly ICouponRepository _couponRepository;
        protected ResponceDto _responce;

        public CouponAPIController(ICouponRepository repository)
        {
            _couponRepository = repository;
            _responce = new ResponceDto();
        }


        [HttpGet("{couponCode}")]
        public async Task<ResponceDto> GetCouponCode(string couponCode)
        {
            try
            {
                var coupon = await _couponRepository.GetCouponByCode(couponCode);
                _responce.Result = coupon;

            }
            catch (Exception ex)
            {
                _responce.IsSucces = false;
                _responce.ErrorMessages = new List<string> { ex.ToString() };
            }
            return _responce;
        }
    }
}
