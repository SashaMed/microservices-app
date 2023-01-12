using AutoMapper;
using CouponAPI.DbContexts;
using CouponAPI.Models.Dtos;
using CouponAPI.Repository.Abstract;
using Microsoft.EntityFrameworkCore;

namespace CouponAPI.Repository
{
    public class CouponRepository : ICouponRepository
    {
        private readonly ApplicationDbContext _context;
        private IMapper _mapper;

        public CouponRepository(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<CouponDto> GetCouponByCode(string code)
        {
            var coupon = await _context.Coupons.FirstOrDefaultAsync(x => x.CouponCode == code);
            return _mapper.Map<CouponDto>(coupon);
        }
    }
}
