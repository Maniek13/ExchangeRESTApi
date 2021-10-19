using ExchangeRESTApi.BaseClasses;
using ExchangeRESTApi.BasesClasses;
using ExchangeRESTApi.Classes;
using ExchangeRESTApi.Tests;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace ExchangeRESTApi
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
            BaseAppKey.SetBaseAppKey(AppConfig.SecretKey);

            //set test 
            SpeedTests speedTests = new SpeedTests("Set data on startup");
            speedTests.SetTime();

            _ = AppConfig.ExchangeValues switch
            {
                "1" => new ExchangeValues(AppConfig.StartDate, AppConfig.EndData),
                "2" => new ExchangeElasticSearch(AppConfig.StartDate, AppConfig.EndData),
                _ => new BaseExchangeValues(),
            };


            //get result
            speedTests.GetResult();

        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews();
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
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=App}/{action=Index}");
            });
        }
    }
}
