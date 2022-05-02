using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Shop.Data.DataContext;
using Shop.DataAccess.DataContext;
using Shop.Domain.Models;
using Shop.UI.Hups;
using Shop.UI.RegisterServices;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);
IServiceCollection services = builder.Services;
// Add services to the container.



services.AddServices(builder.Configuration);


services.ConfigureServiceS();
services.AddMemoryCache();
services.AddSignalR().AddJsonProtocol(options =>
{
    options.PayloadSerializerOptions.PropertyNamingPolicy = null;

});
builder.Services.AddDistributedMemoryCache();
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(
        builder =>
        {
            builder.WithOrigins("https://localhost:44394")
                .AllowAnyHeader()
                .WithMethods("GET", "POST")
                .AllowCredentials();
        });
});

builder.Services.AddSession(options =>
{

    options.IdleTimeout = TimeSpan.FromMinutes(10); //idle time of session (default is 20 mins) before contents in server cache are deleted
    options.Cookie.HttpOnly = true; //cookie not accesible from client-scripting
    options.Cookie.IsEssential = true; //cookie is essential for the app to work
    options.Cookie.Name = ".ShoppingCart.Session"; //set a custom cookie name

});




WebApplication app = builder.Build();






using (var serviceScope = app.Services.CreateScope())
{
    var servicesProvider = serviceScope.ServiceProvider;

    await SeedIdentityUsersAndRoles.Initialize(servicesProvider);
}






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
app.UseCors();
app.UseSession();
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapHub<NotificationsHup>("/noti");

app.Run();
