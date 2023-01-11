using FrontEnd.Models.Dto;
using FrontEnd.Services.IServices;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace FrontEnd.Controllers
{
    public class CartController : Controller
    {
        private readonly IProductService _productService;
        private readonly ICartService _cartService;

        public CartController(IProductService productService, ICartService cartService)
        {
            _productService = productService;
            _cartService = cartService;
        }

        [Authorize]
        public async Task<IActionResult> CartIndex()
        {
            return View(await LoadCartDtoBasedOnLoggedUser());
        }

        public async Task<IActionResult> Remove(int cartDetailsId)
        {
            var token = await HttpContext.GetTokenAsync("access_token");
            var responce = await _cartService.RemoveFromCartAsync<ResponceDto>(cartDetailsId, token);
            return RedirectToAction(nameof(CartIndex));
        }


        private async Task<CartDto> LoadCartDtoBasedOnLoggedUser()
        {
            var userId = User.Claims.Where(u => u.Type == "sub")?.FirstOrDefault().Value;
            var token = await HttpContext.GetTokenAsync("access_token");
            var responce = await _cartService.GetCartbyUserAsync<ResponceDto>(userId, token);
            CartDto cartDto = new();
            if (responce != null && responce.IsSucces)
            {
                cartDto = JsonConvert.DeserializeObject<CartDto>(Convert.ToString(responce.Result));
            }

            if (cartDto.CartHeader != null)
            {
                cartDto.CartHeader.OrderTotal += CalculateOrderTotal(cartDto);
            }

            return cartDto; 
        }

        private double CalculateOrderTotal(CartDto cartDto)
        {
            double sum = 0;
            foreach (var item in cartDto.CartDetails) 
            {
                sum += item.Product.Price * item.Product.Count;
            }
            return sum;
        }
    }
}
