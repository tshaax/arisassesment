using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Aris.ServerTest.Models;

namespace Aris.ServerTest
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            IConfigurationSection sec = Configuration.GetSection("KoreAPI");
            services.Configure<KoreOptions>(sec);

            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => false;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });


            services.AddHttpClient();
            services.AddMvc(options =>
            {
                options.Filters.Add<Filters.ReturnUrlActionFilter>();
                options.Filters.Add<Filters.KoreTokenFilter>();
            }).SetCompatibilityVersion(CompatibilityVersion.Latest);

            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            services.AddTransient<Services.IKoreApiAuthService, Services.KoreApiAuthService>();
            services.AddTransient<Services.IKoreApiGameService, Services.KoreApiGameService>();
            services.AddTransient<Services.IKoreApiUserService, Services.KoreApiUserService>();
            services.AddTransient<Services.IKoreClaimsService, Services.KoreClaimsService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy();
            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
