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
using WheelOfFortune.Additionals;
using WheelOfFortune.Data;
using WheelOfFortune.Models;
using WheelOfFortune.Services;

namespace WheelOfFortune
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
               
               services.AddIdentity<ApplicationUser, IdentityRole>(options =>
               {
                    // Password settings
                    options.Password.RequireDigit = true;
                    options.Password.RequiredLength = 8;
                    options.Password.RequireNonAlphanumeric = false;
                    options.Password.RequireUppercase = false;
                    options.Password.RequireLowercase = true;
                    options.Password.RequiredUniqueChars = 2;
                    // Lockout settings
                    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(1);
                    options.Lockout.MaxFailedAccessAttempts = 0; //3
                    options.Lockout.AllowedForNewUsers = false;//true
                    // Signin settings
                    options.SignIn.RequireConfirmedEmail = false;//true
                    options.SignIn.RequireConfirmedPhoneNumber = false;
               })
                   .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();

            // Add application services.
            services.AddTransient<IEmailSender, EmailSender>();
               services.Configure<AuthMessageSenderOptions>(Configuration);

            services.AddMvc();
            services.AddCors();
            //services.AddSignalR();

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
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

            /*app.UseSignalR(routes =>
            {
                routes.MapHub<UserBalanceUpdated>("userbalanceupdated");
            });*/

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
        }

          
     }
}
