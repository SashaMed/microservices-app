using FrontEnd.Models.Dto;
using FrontEnd.Services.IServices;
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
			var responce = await _productService.GetAllProductsAsync<ResponceDto>();
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
				var responce = await _productService.CreateProductsAsync<ResponceDto>(model);
				if (responce != null && responce.IsSucces)
				{
					return RedirectToAction(nameof(ProductIndex));
				}
			}
            return View(model);
        }


        public async Task<IActionResult> ProductEdit(int productId)
        {
            var responce = await _productService.GetProductByIdAsync<ResponceDto>(productId);
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
                var responce = await _productService.UpdateProductsAsync<ResponceDto>(model);
                if (responce != null && responce.IsSucces)
                {
                    return RedirectToAction(nameof(ProductIndex));
                }
            }
            return View(model);
        }


        public async Task<IActionResult> ProductDelete(int productId)
        {
            var responce = await _productService.GetProductByIdAsync<ResponceDto>(productId);
            if (responce != null && responce.IsSucces)
            {
                var productDto = JsonConvert.DeserializeObject<ProductDto>(Convert.ToString(responce.Result));
                return View(productDto);
            }
            return NotFound();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ProductDelete(ProductDto model)
        {

            var responce = await _productService.DeleteProductsAsync<ResponceDto>(model.ProductId);
            if (responce != null && responce.IsSucces)
            {
                return RedirectToAction(nameof(ProductIndex));
            }

            return View(model);
        }

    }
}
