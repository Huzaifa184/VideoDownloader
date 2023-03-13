
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;


var builder = WebApplication.CreateBuilder(args);


builder.WebHost.ConfigureServices(services =>
{
    
    services.AddHttpContextAccessor();
});


builder.Services.AddRazorPages();
builder.Services.AddDbContext<MyDbContext>(o => o.UseSqlite("filename=urls.db"));


builder.Services.AddSession(options =>
{
    
    options.Cookie.Name = ".MyApp.Session";
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});


builder.Services.AddHttpClient();


var app = builder.Build();


if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}


app.UseHttpsRedirection();


app.UseStaticFiles();


app.UseRouting();


app.UseSession();


app.UseAuthorization();


app.MapRazorPages();


app.Run();
