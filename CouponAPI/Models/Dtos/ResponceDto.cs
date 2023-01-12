using System.Text.Json;

namespace CouponAPI.Models.Dtos
{
    public class ResponceDto
    {
        public bool IsSucces { get; set; } = true;
        public object Result { get; set; }
        public string DisplayMessage { get; set; } = string.Empty;
        public List<string> ErrorMessages { get; set; }

        public override string ToString() => JsonSerializer.Serialize(this);
    }
}
