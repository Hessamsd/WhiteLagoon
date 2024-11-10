using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using WhiteLagoon.Application.Common.Interfaces;
using WhiteLagoon.Domain.Entities;
using WhiteLagoon.Infrastructure.Data;
using WhiteLagoon.Infrastructure.Repository;

public class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        builder.Services.AddControllersWithViews();
        builder.Services.AddControllers();





        builder.Services.AddDbContext<ApplicationDbContext>(option =>
        option.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));



        builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
            .AddEntityFrameworkStores<ApplicationDbContext>();//AddDefaultTokenProviders();


        builder.Services.Configure<IdentityOptions>(option =>
        {

            option.Password.RequiredLength = 6;

        });




        builder.Services.ConfigureApplicationCookie(option =>
        {
            option.AccessDeniedPath = "/Account/AccessDenied";
            option.LoginPath = "/Account/Login";

        });




        builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();






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
        //app.UseWebSockets();
        app.UseRouting();
        app.UseAuthorization();




        app.MapControllerRoute(
            name: "default",
            pattern: "{controller=Home}/{action=Index}/{id?}");


        app.MapControllers();


        app.Run();
    }
}