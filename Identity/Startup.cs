using Duende.IdentityServer.AspNetIdentity;
using Duende.IdentityServer.Services;
using Identity.DbContexts;
using Identity.Initializers;
using Identity.Models;
using Identity.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Identity
{
    public class Startup
    {

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; private set; }



        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IDbInitializer dbInitializer)
        {
            // Configure the HTTP request pipeline.
            if (env.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseIdentityServer();
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}")
                );

            dbInitializer.Initialize();
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews();
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));
            services.AddIdentity<ApplicationUser, IdentityRole>().
                AddEntityFrameworkStores<ApplicationDbContext>().AddDefaultTokenProviders();
            var identityServerBuilder = services.AddIdentityServer(options =>
            {
                options.Events.RaiseFailureEvents = true;
                options.Events.RaiseSuccessEvents = true;
                options.Events.RaiseErrorEvents = true;
                options.Events.RaiseInformationEvents = true;
                options.EmitStaticAudienceClaim = true;
            }).AddInMemoryIdentityResources(StaticData.IdentityResources)
            .AddInMemoryApiScopes(StaticData.ApiScopes)
            .AddInMemoryClients(StaticData.Clients)
            .AddAspNetIdentity<ApplicationUser>();

            identityServerBuilder.AddDeveloperSigningCredential();
            services.AddScoped<IProfileService, ProfileService>();
            services.AddScoped<IDbInitializer, DbInitializer>();
        }
    }
}
