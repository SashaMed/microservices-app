//using Identity.DbContexts;
//using Identity.Initializers;
//using Identity.Models;
//using Microsoft.AspNetCore.Identity;
//using Microsoft.EntityFrameworkCore;

//namespace Identity
//{
//    public class Program
//    {
//        public static void Main(string[] args)
//        {
//            var builder = WebApplication.CreateBuilder(args);

//            // Add services to the container.
//            builder.Services.AddControllersWithViews();
//            builder.Services.AddDbContext<ApplicationDbContext>(options =>
//                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
//            builder.Services.AddIdentity<ApplicationUser, IdentityRole>().
//                AddEntityFrameworkStores<ApplicationDbContext>().AddDefaultTokenProviders();
//            var identityServerBuilder = builder.Services.AddIdentityServer(options =>
//            {
//                options.Events.RaiseFailureEvents = true;
//                options.Events.RaiseSuccessEvents = true;
//                options.Events.RaiseErrorEvents = true;
//                options.Events.RaiseInformationEvents = true;
//                options.EmitStaticAudienceClaim = true;
//            }).AddInMemoryIdentityResources(StaticData.IdentityResources)
//            .AddInMemoryApiScopes(StaticData.ApiScopes)
//            .AddInMemoryClients(StaticData.Clients)
//            .AddAspNetIdentity<ApplicationUser>();

//            identityServerBuilder.AddDeveloperSigningCredential();

//            builder.Services.AddScoped<IDbInitializer, DbInitializer>();

//            var app = builder.Build();

//            // Configure the HTTP request pipeline.
//            if (!app.Environment.IsDevelopment())
//            {
//                app.UseExceptionHandler("/Home/Error");
//                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
//                app.UseHsts();
//            }
//            app.UseIdentityServer();
//            app.UseHttpsRedirection();
//            app.UseStaticFiles();

//            app.UseRouting();

//            app.UseAuthorization();

//            app.MapControllerRoute(
//                name: "default",
//                pattern: "{controller=Home}/{action=Index}/{id?}");

//            app.Run();
//        }
//    }
//}


using Identity;
using Microsoft.AspNetCore.Hosting;

public class Program
{
    public static void Main(string[] args)
    {
        CreateHostBuilder(args).Build().Run();
    }


    public static IHostBuilder CreateHostBuilder(string[] args) =>
        Host.CreateDefaultBuilder(args).ConfigureWebHostDefaults(webBuilder =>
        {
            webBuilder.UseStartup<Startup>();
        });
}