namespace FrontEnd
{
	public static class StaticData
	{
		public static string ProductAPIBase { get; set; }

        public static string ShoppingCartAPIBase { get; set; }

        public static string CouponAPIBase { get; set; }

        public enum ApiType
		{
			GET,
			POST, 
			PUT, 
			DELETE
		}
	}
}
