using ShoppingCartAPI.Models.Dtos;

namespace ShoppingCartAPI.Repository.Abstract
{
    public interface ICartRepository
    {
        Task<CartDto> GetCartByUserId(string userId);

        Task<CartDto> CreateUpdateCart(CartDto cartDto);
        Task<bool> RemoveFromCart(int cartDetailsId);

        Task<bool> ClearCart(string userId);
    }
}
