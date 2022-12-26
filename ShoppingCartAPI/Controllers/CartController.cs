using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ShoppingCartAPI.Models.Dtos;
using ShoppingCartAPI.Repository.Abstract;

namespace ShoppingCartAPI.Controllers
{
    [Route("api/cart")] 
    [ApiController]
    public class CartController : ControllerBase
    {
        private readonly ICartRepository _cartRepository;
        protected ResponceDto _responce;

        public CartController(ICartRepository repository)
        {
            _cartRepository = repository;
            _responce = new ResponceDto();
        }

        [HttpGet("GetCart/{userId}")]
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



        [HttpPost("AddCart")]
        public async Task<ResponceDto> AddCart(CartDto cartDto)
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


        [HttpPost("UpdateCart")]
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


        [HttpPost("RemoveCart")]
        public async Task<ResponceDto> RemoveCart([FromBody]int cartId)
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


        [HttpPost("ClearCart/{userId}")]
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
    }
}
