using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using WheelOfFortune.Admin.Data;
using WheelOfFortune.Admin.Models;
using WheelOfFortune.Admin.Services;
using Swashbuckle.AspNetCore.Swagger;
using Microsoft.AspNetCore.Cors;
using WheelOfFortune.Admin.Classes.Interfaces;
using WheelOfFortune.Admin.Classes.Implementations;

namespace WheelOfFortune.Admin
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
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

            services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();

            // Add application services.
            services.AddTransient<IEmailSender, EmailSender>();

            services.AddMvc();

               // Register the Swagger generator, defining one or more Swagger documents
               //services.AddSwaggerGen(c =>
               //{
               //     c.SwaggerDoc("v1", new Info { Title = "My API", Version = "v1" });
               //});

               //Add Cross-origin resource sharing to avoid problems calling localhost resources from localhost
               services.AddCors();

          }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public async void Configure(IApplicationBuilder app, IHostingEnvironment env,
                              RoleManager<IdentityRole> rolemanager)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseBrowserLink();
                app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseStaticFiles();

            app.UseAuthentication();

               //Enable middleware to serve generated Swagger as a JSON endpoint.
               //app.UseSwagger();

               //Enable middleware to serve swagger - ui(HTML, JS, CSS, etc.), specifying the Swagger JSON endpoint.
               //app.UseSwaggerUI(c =>
               //{
               //     c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
               //});

               //Shows UseCors with CorsPolicyBuilder.
               app.UseCors(builder =>
                  builder.WithOrigins("*") //Use these settings for localhost testing only
                  .AllowAnyHeader()
                  );

            app.UseMvc(routes =>
            {
                 routes.MapRoute(
                        name: "default",
                        template: "{controller=Home}/{action=Index}/{id?}");
            });

               //await Initializer.initial(rolemanager);
          }
    }
}
