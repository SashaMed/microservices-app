﻿using ShoppingCartAPI.Models.Dtos;

namespace ShoppingCartAPI.Messages
{
    public class CheckoutHeaderDto
    {
        public int CartHeaderId { get; set; }

        public string UserId { get; set; }
        public string CouponCode { get; set; }

        public double OrderTotal { get; set; }

        public double DiscountTotal { get; set; }
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Phone { get; set; }

        public string Email { get; set; }

        public DateTime PickupDateTime { get; set; }

        public string CardNumber { get; set; }

        public string CVV { get; set; }

        public string ExpiryMonth { get; set; }

        public int CardTotalItems { get; set; }
        public IEnumerable<CartDetailsDto> CartDetails { get; set; }
    }
}
