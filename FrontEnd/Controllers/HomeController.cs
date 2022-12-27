using FrontEnd.Models;
using FrontEnd.Models.Dto;
using FrontEnd.Services.IServices;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Diagnostics;

namespace FrontEnd.Controllers
{
	public class HomeController : Controller
	{
		private readonly ILogger<HomeController> _logger;
		private readonly IProductService _productService;
		private readonly ICartService _cartService;

		public HomeController(ILogger<HomeController> logger, IProductService productService, ICartService cartService)
		{
			_logger = logger;
			_productService = productService;
			_cartService = cartService;
		}

		public async Task<IActionResult> Index()
		{
			List<ProductDto> list = new();
			var responce = await _productService.GetAllProductsAsync<ResponceDto>("");
			if (responce != null && responce.IsSucces)
			{
				list = JsonConvert.DeserializeObject<List<ProductDto>>(Convert.ToString(responce.Result));
			}
			return View(list);
		}

		[Authorize]
        public async Task<IActionResult> Details(int productId)
        {
            ProductDto model = new();
            var token = await HttpContext.GetTokenAsync("access_token");
            var responce = await _productService.GetProductByIdAsync<ResponceDto>(productId, token);
            if (responce != null && responce.IsSucces)
            {
                model = JsonConvert.DeserializeObject<ProductDto>(Convert.ToString(responce.Result));
            }
            return View(model);
        }

        [HttpPost]
        [ActionName("Details")]
        [Authorize]
        public async Task<IActionResult> DetailsPost(ProductDto productDto)
        {
			var cartDto = new CartDto
			{
				CartHeader = new CartHeaderDto()
				{
					UserId = User.Claims.Where(u => u.Type == "sub")?.FirstOrDefault()?.Value
				}
				
			};
			var cartDetailsDto = new CartDetailsDto
			{
				Count = productDto.Count,
				ProductId = productDto.ProductId
			};

			var resp = await _productService.GetProductByIdAsync<ResponceDto>(productDto.ProductId, "");
			if (resp != null && resp.IsSucces)
			{
				cartDetailsDto.Product = JsonConvert.DeserializeObject<ProductDto>(Convert.ToString(resp.Result));
			}

			List<CartDetailsDto> cartDetails= new List<CartDetailsDto>();
			cartDetails.Add(cartDetailsDto);
			cartDto.CartDetails = cartDetails;

            var token = await HttpContext.GetTokenAsync("access_token");
			var addToCartRes = await _cartService.AddToCartAsync<ResponceDto>(cartDto, token);
            if (addToCartRes != null && addToCartRes.IsSucces)
            {
				return RedirectToAction(nameof(Index));
            }
			ModelState.AddModelError(string.Empty, "Cant add product to shopping cart");
            return View(productDto);
        }

        public IActionResult Privacy()
		{
			return View();
		}


		[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
		public IActionResult Error()
		{
			return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
		}

		[Authorize]
        public IActionResult Login()
        {
			return RedirectToAction(nameof(Index));
        }

        public IActionResult Logout()
        {
			return SignOut("Cookies", "oidc");
        }

    }
}