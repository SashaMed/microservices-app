using AutoMapper;
using Microsoft.EntityFrameworkCore;
using ShoppingCartAPI.DbContexts;
using ShoppingCartAPI.Models;
using ShoppingCartAPI.Models.Dtos;
using ShoppingCartAPI.Repository.Abstract;

namespace ShoppingCartAPI.Repository
{
    public class CartRepository : ICartRepository
    {
        private readonly ApplicationDbContext _context;
        private IMapper _mapper;

        public CartRepository(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<bool> ClearCart(string userId)
        {
            var cartHeader = await _context.CartHeaders.FirstOrDefaultAsync(c => c.UserId == userId);
            if (cartHeader != null)
            {
                _context.CartDetails.
                    RemoveRange(_context.CartDetails.Where(c => c.CartHeaderId == cartHeader.CartHeaderId));
                _context.CartHeaders.Remove(cartHeader);
                await _context.SaveChangesAsync();  
                return true;
            }
            return false;
        }

        public async Task<CartDto> CreateUpdateCart(CartDto cartDto)
        {
            var cart = _mapper.Map<Cart>(cartDto);
            var prodIdDb = await _context.Products
                .FirstOrDefaultAsync(c => c.ProductId == cartDto.CartDetails.First().ProductId);
            if (prodIdDb == null)
            {
                _context.Products.Add(cart.CartDetails.FirstOrDefault().Product);
                await _context.SaveChangesAsync();
            }

            var cartHeaderFromDb = await _context.CartHeaders.AsNoTracking()
                .FirstOrDefaultAsync(c => c.UserId == cart.CartHeader.UserId);
            if (cartHeaderFromDb == null)
            {
                _context.CartHeaders.Add(cart.CartHeader);
                await _context.SaveChangesAsync();
                cart.CartDetails.FirstOrDefault().CartHeaderId = cart.CartHeader.CartHeaderId;
                cart.CartDetails.FirstOrDefault().Product = null;
                _context.CartDetails.Add(cart.CartDetails.FirstOrDefault());
                await _context.SaveChangesAsync();
            }
            else
            {
                var cartDetailsFromDb = await _context.CartDetails.AsNoTracking().FirstOrDefaultAsync(
                    c => c.ProductId == cart.CartDetails.FirstOrDefault().ProductId
                    && c.CartHeaderId == cartHeaderFromDb.CartHeaderId);
                if (cartDetailsFromDb == null)
                {
                    cart.CartDetails.FirstOrDefault().CartHeaderId = cartHeaderFromDb.CartHeaderId;
                    cart.CartDetails.FirstOrDefault().Product = null;
                    _context.CartDetails.Add(cart.CartDetails.FirstOrDefault());
                    await _context.SaveChangesAsync();
                }
                else
                {
                    cart.CartDetails.FirstOrDefault().Count += cartDetailsFromDb.Count;
                    _context.CartDetails.Update(cart.CartDetails.FirstOrDefault());
                    await _context.SaveChangesAsync();
                }
            }
            return _mapper.Map<CartDto>(cart);
        }

        public async Task<CartDto> GetCartByUserId(string userId)
        {
            var cart = new Cart
            {
                CartHeader = await _context.CartHeaders.FirstOrDefaultAsync(c => c.UserId == userId)
            };
            cart.CartDetails = _context.CartDetails
                .Where(c => c.CartHeaderId == cart.CartHeader.CartHeaderId)
                .Include(u => u.Product);
            return  _mapper.Map<CartDto>(cart);
        }

        public async Task<bool> RemoveFromCart(int cartDetailsId)
        {
            try
            {
                var cartDetail = await _context.CartDetails
                    .FirstOrDefaultAsync(c => c.CartDetailsId == cartDetailsId);

                int countOfDetails = await _context.CartDetails
                    .Where(c => c.CartHeaderId == cartDetail.CartHeaderId).CountAsync();

                _context.CartDetails.Remove(cartDetail);
                if (countOfDetails == 1)
                {
                    var cartHeaderToRemove = await _context.CartHeaders
                        .FirstOrDefaultAsync(c => c.CartHeaderId == cartDetail.CartHeaderId);
                    _context.CartHeaders.Remove(cartHeaderToRemove);
                }
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex) 
            {
                return false;
            }
        }
    }
}
