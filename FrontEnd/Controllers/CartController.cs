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
        private readonly ICouponService _couponService;

        public CartController(IProductService productService, ICartService cartService, ICouponService couponService)
        {
            _productService = productService;
            _cartService = cartService;
            _couponService = couponService;
        }

        [Authorize]
        public async Task<IActionResult> CartIndex()
        {
            return View(await LoadCartDtoBasedOnLoggedUser());
        }

        [HttpPost]
        [ActionName("ApplyCoupon")]
        public async Task<IActionResult> ApplyCoupon(CartDto cartDto)
        {
            var userId = User.Claims.Where(u => u.Type == "sub")?.FirstOrDefault().Value;
            var token = await HttpContext.GetTokenAsync("access_token");
            var responce = await _cartService.ApplyCoupon<ResponceDto>(cartDto, token);
            return RedirectToAction(nameof(CartIndex));
        }


        [HttpPost]
        [ActionName("RemoveCoupon")]
        public async Task<IActionResult> RemoveCoupon(CartDto cartDto)
        {
            var userId = User.Claims.Where(u => u.Type == "sub")?.FirstOrDefault().Value;
            var token = await HttpContext.GetTokenAsync("access_token");
            var responce = await _cartService.RemoveCoupon<ResponceDto>(cartDto.CartHeader.UserId, token);
            return RedirectToAction(nameof(CartIndex));
        }

        public async Task<IActionResult> Remove(int cartDetailsId)
        {
            var token = await HttpContext.GetTokenAsync("access_token");
            var responce = await _cartService.RemoveFromCartAsync<ResponceDto>(cartDetailsId, token);
            return RedirectToAction(nameof(CartIndex));
        }

        [Authorize] 
        public async Task<IActionResult> Checkout()
        {
            return View(await LoadCartDtoBasedOnLoggedUser());
        }


        public async Task<IActionResult> Confirmation()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Checkout(CartDto cartDto)
        {
            try
            {
                var token = await HttpContext.GetTokenAsync("access_token");
                var responce = await _cartService.Checkout<ResponceDto>(cartDto.CartHeader, token);
                return RedirectToAction(nameof(Confirmation));
            }
            catch (Exception ex)
            {
                return View(cartDto);
            }
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
                if (!string.IsNullOrEmpty(cartDto.CartHeader.CouponCode)) 
                {
                    var couponResponce = await _couponService.GetCoupon<ResponceDto>(cartDto.CartHeader.CouponCode, token);
                    if (couponResponce != null && couponResponce.IsSucces)
                    {
                        var coupon = JsonConvert.DeserializeObject<CouponDto>(Convert.ToString(couponResponce.Result));
                        cartDto.CartHeader.DiscountTotal = coupon.DiscountAmount;
                    }
                }
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
            sum -= cartDto.CartHeader.DiscountTotal;
            return sum;
        }
    }
}
