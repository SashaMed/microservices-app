﻿using FrontEnd.Models.Dto;

namespace FrontEnd.Services.IServices
{
    public interface ICartService
    {
        Task<T> GetCartbyUserAsync<T>(string userId, string token = null);

        Task<T> AddToCartAsync<T>(CartDto cartDto, string token = null);

        Task<T> UpdateCartAsync<T>(CartDto cartDto, string token = null);

        Task<T> RemoveFromCartAsync<T>(int cartId, string token = null);
    }
}