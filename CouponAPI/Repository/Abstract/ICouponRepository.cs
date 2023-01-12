using CouponAPI.Models.Dtos;

namespace CouponAPI.Repository.Abstract
{
    public interface ICouponRepository
    {
        Task<CouponDto> GetCouponByCode(string code);
    }
}
