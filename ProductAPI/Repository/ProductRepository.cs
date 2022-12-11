using AutoMapper;
using Microsoft.EntityFrameworkCore;
using ProductAPI.DbContexts;
using ProductAPI.Models;
using ProductAPI.Models.Dto;

namespace ProductAPI.Repository
{
	public class ProductRepository : IProductRepository
	{
		private readonly ApplicationDbContext _context;
		private IMapper _mapper;

		public ProductRepository(ApplicationDbContext context, IMapper mapper)
		{
			_context = context;	
			_mapper = mapper;
		}

		public async Task<ProductDto> CreateUpdateProduct(ProductDto productDto)
		{
			var product = _mapper.Map<ProductDto, Product>(productDto);
			if (product.ProductId > 0)
			{
				_context.Update(product);
			}
			else
			{
				_context.Products.Add(product);
			}
			await _context.SaveChangesAsync();
			return _mapper.Map<Product, ProductDto>(product);
		}

		public async Task<bool> DeleteProduct(int productId)
		{
			try
			{
				var product = await _context.Products.FirstOrDefaultAsync(b => b.ProductId == productId);
				if (product == null)
				{
					return false;
				}
				_context.Products.Remove(product);
				await _context.SaveChangesAsync();
				return true;
			}
			catch (Exception ex)
			{
				return false;
			}
		}

		public async Task<ProductDto> GetProductById(int productId)
		{
			var product = await _context.Products.Where(b => b.ProductId == productId).FirstOrDefaultAsync();
			return _mapper.Map<ProductDto>(product);
		}

		public async Task<IEnumerable<ProductDto>> GetProducts()
		{
			IEnumerable <Product> productList = await _context.Products.ToListAsync();
			return _mapper.Map<List<ProductDto>>(productList);
		}
	}
}
