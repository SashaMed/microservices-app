using FrontEnd.Services;
using FrontEnd.Services.IServices;
using Microsoft.AspNetCore.Authentication;

namespace FrontEnd
{
	public class Program
	{
		public static void Main(string[] args)
		{
			var builder = WebApplication.CreateBuilder(args);

			// Add services to the container.
			builder.Services.AddHttpClient<IProductService, ProductService>();
            builder.Services.AddHttpClient<ICartService, CartService>();
            StaticData.ProductAPIBase = builder.Configuration["ServicesUrls:ProductAPI"];
            StaticData.ShoppingCartAPIBase = builder.Configuration["ServicesUrls:ShoppingCartAPI"];
            builder.Services.AddScoped<IProductService, ProductService>();
            builder.Services.AddScoped<ICartService, CartService>();
            builder.Services.AddControllersWithViews();

			builder.Services.AddAuthentication(options =>
			{
				options.DefaultScheme = "Cookies";
				options.DefaultChallengeScheme = "oidc";
			})
				.AddCookie("Cookies", c => c.ExpireTimeSpan = TimeSpan.FromMinutes(10))
				.AddOpenIdConnect("oidc", options =>
				{
					options.Authority = builder.Configuration["ServicesUrls:IdentityAPI"];
					options.GetClaimsFromUserInfoEndpoint = true;
					options.ClientId = "mango";
					options.ClientSecret = "secret";
					options.ResponseType = "code";
					options.ClaimActions.MapJsonKey("role", "role", "role");
                    options.ClaimActions.MapJsonKey("sub", "sub", "sub");
                    options.TokenValidationParameters.NameClaimType = "name";
					options.TokenValidationParameters.RoleClaimType = "role";
					options.Scope.Add("mango");
					options.SaveTokens = true; 
				});


			var app = builder.Build();

			// Configure the HTTP request pipeline.
			if (!app.Environment.IsDevelopment())
			{
				app.UseExceptionHandler("/Home/Error");
				// The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
				app.UseHsts();
			}

			app.UseHttpsRedirection();
			app.UseStaticFiles();

			app.UseRouting();
			app.UseAuthentication();
			app.UseAuthorization();

			app.MapControllerRoute(
				name: "default",
				pattern: "{controller=Home}/{action=Index}/{id?}");

			app.Run();
		}
	}
}