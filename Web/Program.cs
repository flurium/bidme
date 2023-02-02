using Bll.Infrastructure;
using Dal.Context;
using Domain.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

/// DB
builder.Services.AddDbContext<BidMeDbContext>(options => options.UseNpgsql(connectionString)); // (connectionString, new MySqlServerVersion(new Version(8, 0, 31))));

// AUTH
builder.Services.AddIdentity<User, IdentityRole>(options => options.SignIn.RequireConfirmedEmail = true)
   .AddEntityFrameworkStores<BidMeDbContext>().AddDefaultTokenProviders().AddTokenProvider<EmailConfirmationTokenProvider<User>>("emailConfirmationProvider");

var logger = new LoggerConfiguration()
    .WriteTo.File("./logs.json", rollingInterval: RollingInterval.Day)
    .MinimumLevel.Fatal()
    .CreateLogger();

builder.Logging.AddSerilog(logger);

builder.Services.Configure<EmailConfirmationProviderOptions>(options => options.TokenLifespan = TimeSpan.FromDays(1));

BllConfiguration.ConfigureServices(builder.Services);


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

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapControllerRoute(
    name: "emailConfirmation",
    pattern: "confirmation/",
    defaults: new { controller = "EmailConfirm", action = "Confirm" });

app.Run();