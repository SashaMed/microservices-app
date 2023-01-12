using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ShoppingCartAPI.Models.Dtos;
using ShoppingCartAPI.Repository.Abstract;

namespace ShoppingCartAPI.Controllers
{
    [Route("api/carts")] 
    [ApiController]
    public class CartAPIController : ControllerBase
    {
        private readonly ICartRepository _cartRepository;
        protected ResponceDto _responce;

        public CartAPIController(ICartRepository repository)
        {
            _cartRepository = repository;
            _responce = new ResponceDto();
        }

        [HttpGet("{userId}")]
        public async Task<ResponceDto> GetCart(string userId)
        {
            try
            {
                var cartDto = await _cartRepository.GetCartByUserId(userId);
                _responce.Result = cartDto;

            }
            catch (Exception ex) 
            {
                _responce.IsSucces = false;
                _responce.ErrorMessages = new List<string> { ex.ToString() };
            }
            return _responce;
        }



        [HttpPost]
        [Authorize]
        public async Task<ResponceDto> AddCart([FromBody] CartDto cartDto)
        {
            try
            {
                var result = await _cartRepository.CreateUpdateCart(cartDto);
                _responce.Result = result;
            }
            catch (Exception ex)
            {
                _responce.IsSucces = false;
                _responce.ErrorMessages = new List<string> { ex.ToString() };
            }
            return _responce;
        }


        [HttpPut]
        public async Task<ResponceDto> UpdateCart(CartDto cartDto)
        {
            try
            {
                var result = await _cartRepository.CreateUpdateCart(cartDto);
                _responce.Result = result;

            }
            catch (Exception ex)
            {
                _responce.IsSucces = false;
                _responce.ErrorMessages = new List<string> { ex.ToString() };
            }
            return _responce;
        }


        [HttpPost("{cartId}")]
        public async Task<ResponceDto> RemoveCart(int cartId)
        {
            try
            {
                var result = await _cartRepository.RemoveFromCart(cartId);
                _responce.Result = result;

            }
            catch (Exception ex)
            {
                _responce.IsSucces = false;
                _responce.ErrorMessages = new List<string> { ex.ToString() };
            }
            return _responce;
        }


        [HttpDelete("{userId}")]
        public async Task<ResponceDto> ClearCart(string userId)
        {
            try
            {
                var result = await _cartRepository.ClearCart(userId);
                _responce.Result = result;

            }
            catch (Exception ex)
            {
                _responce.IsSucces = false;
                _responce.ErrorMessages = new List<string> { ex.ToString() };
            }
            return _responce;
        }


        [HttpPost("remove-coupon/{userId}")] 
        public async Task<ResponceDto> RemoveCoupon(string userId)
        {
            try
            {
                var result = await _cartRepository.RemoveCoupon(userId);
                _responce.Result = result;

            }
            catch (Exception ex)
            {
                _responce.IsSucces = false;
                _responce.ErrorMessages = new List<string> { ex.ToString() };
            }
            return _responce;
        }


        [HttpPost("apply-coupon")]
        public async Task<ResponceDto> ApplyCoupon([FromBody] CartDto cartDto)
        {
            try
            {
                var result = await _cartRepository.ApplyCoupon(cartDto.CartHeader.UserId, cartDto.CartHeader.CouponCode);
                _responce.Result = result;

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
