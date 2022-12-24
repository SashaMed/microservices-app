using FrontEnd.Models.Dto;
using FrontEnd.Services.IServices;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Reflection;

namespace FrontEnd.Controllers
{
	public class ProductController : Controller
	{ 
		private IProductService _productService;

		public ProductController(IProductService productService)
		{
			_productService = productService;
		}

		public async Task<IActionResult> ProductIndex()
		{
			var list = new List<ProductDto>();
            var token = await HttpContext.GetTokenAsync("access_token");
			var responce = await _productService.GetAllProductsAsync<ResponceDto>(token);
			if (responce != null && responce.IsSucces)
			{
				list = JsonConvert.DeserializeObject<List<ProductDto>>(Convert.ToString(responce.Result));
			}

			return View(list);
		}

        public IActionResult ProductCreate()
        {
            return View();
        }

		[HttpPost]
		[ValidateAntiForgeryToken]
        public async Task<IActionResult> ProductCreate(ProductDto model)
        {
			if (ModelState.IsValid)
			{
                var token = await HttpContext.GetTokenAsync("access_token");
                var responce = await _productService.CreateProductsAsync<ResponceDto>(model, token);
				if (responce != null && responce.IsSucces)
				{
					return RedirectToAction(nameof(ProductIndex));
				}
			}
            return View(model);
        }


        public async Task<IActionResult> ProductEdit(int productId)
        {
            var token = await HttpContext.GetTokenAsync("access_token");
            var responce = await _productService.GetProductByIdAsync<ResponceDto>(productId, token);
            if (responce != null && responce.IsSucces)
            {
                var productDto = JsonConvert.DeserializeObject<ProductDto>(Convert.ToString(responce.Result));
                return View(productDto);
            }
            return NotFound();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ProductEdit(ProductDto model)
        {
            if (ModelState.IsValid)
            {
                var token = await HttpContext.GetTokenAsync("access_token");
                var responce = await _productService.UpdateProductsAsync<ResponceDto>(model, token);
                if (responce != null && responce.IsSucces)
                {
                    return RedirectToAction(nameof(ProductIndex));
                }
            }
            return View(model);
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> ProductDelete(int productId)
        {
            var token = await HttpContext.GetTokenAsync("access_token");
            var responce = await _productService.GetProductByIdAsync<ResponceDto>(productId, token);
            if (responce != null && responce.IsSucces)
            {
                var productDto = JsonConvert.DeserializeObject<ProductDto>(Convert.ToString(responce.Result));
                return View(productDto);
            }
            return NotFound();
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ProductDelete(ProductDto model)
        {
            var token = await HttpContext.GetTokenAsync("access_token");
            var responce = await _productService.DeleteProductsAsync<ResponceDto>(model.ProductId, token);
            if (responce != null && responce.IsSucces)
            {
                return RedirectToAction(nameof(ProductIndex));
            }

            return View(model);
        }

    }
}
