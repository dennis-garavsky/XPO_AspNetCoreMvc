using System;
using DevExpress.Xpo;
using DevExpress.Xpo.Metadata;
using DevExtremeAspNetCoreApp1.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using DevExtremeAspNetCoreApp1.Helpers;

namespace DevExtremeAspNetCoreApp1
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services) {
            services
                .AddMvc()
                .AddDxSampleModelJsonOptions();
            services
                .AddXpoDefaultUnitOfWork(true, sp => CreateDataLayer());
        }

        IDataLayer CreateDataLayer() {
            string connectionString = Configuration["ConnectionString"];
            var provider = XpoDefault.GetConnectionProvider(connectionString, DevExpress.Xpo.DB.AutoCreateOption.DatabaseAndSchema);
            var dictionary = new ReflectionDictionary();
            dictionary.GetDataStoreSchema(typeof(Customer), typeof(Order));
            return new ThreadSafeDataLayer(dictionary, provider);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseBrowserLink();
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseStaticFiles();
            app.UseXpoDemoData();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
