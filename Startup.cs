using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Reflection;
using WebApp.Observer.Models;
using WebApp.Observer.Observer;


namespace WebApp.Observer;


public class Startup
{
    public IConfiguration Configuration { get; }

    public Startup(IConfiguration configuration) => Configuration = configuration;



    public void ConfigureServices(IServiceCollection services)
    {
        services.AddSingleton(sp =>
        {
            UserObserverSubject userObserverSubject = new();

            userObserverSubject.RegisterObserver(new UserObserverWriteToConsole(sp));
            userObserverSubject.RegisterObserver(new UserObserverCreateDiscount(sp));
            userObserverSubject.RegisterObserver(new UserObserverSendEmail(sp));

            return userObserverSubject;
        });

        services.AddMediatR(Assembly.GetExecutingAssembly());

        services.AddDbContext<AppIdentityDbContext>(options =>
        {
            options.UseSqlServer(Configuration.GetConnectionString("SqlServer"));
        });

        services.AddIdentity<AppUser, IdentityRole>(options =>
        {
            options.User.RequireUniqueEmail = true;
        }).AddEntityFrameworkStores<AppIdentityDbContext>();

        services.AddControllersWithViews();
    }


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

        app.UseRouting();
        app.UseAuthentication();
        app.UseAuthorization();

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");
        });
    }
}