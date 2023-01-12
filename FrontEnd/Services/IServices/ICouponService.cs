namespace FrontEnd.Services.IServices
{
    public interface ICouponService
    {
        Task<T> GetCoupon<T>(string coupon, string toke = null);
    }
}
