using Core.DomainServices.Repos.Intf;
using Core.DomainServices.Services.Intf;
using Core.DomainServices.Services.Impl;
using Infrastructure.TMEF;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("Default");
builder.Services.AddDbContext<GameDbContext>(options => options.UseSqlServer(connectionString));

var userConnectionString = builder.Configuration.GetConnectionString("Security");
builder.Services.AddDbContext<SecurityDbContext>(options => options.UseSqlServer(userConnectionString));

builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddIdentity<IdentityUser, IdentityRole>(options => options.SignIn.RequireConfirmedAccount = false)
    .AddEntityFrameworkStores<SecurityDbContext>();

builder.Services.AddAuthorization(options =>
    options.AddPolicy("TeamManagerOnly", policy => policy.RequireClaim("TeamManager")));

builder.Services.AddScoped<IGameService, GameServiceBasic>();
builder.Services.AddScoped<IGameRepository, GameEFRepository>();
builder.Services.AddScoped<ICoachRepository, CoachEFRepository>();
builder.Services.AddScoped<IPlayerRepository, PlayerEFRepository>();
builder.Services.AddScoped<IOpponentRepository, OpponentEFRepository>();
builder.Services.AddScoped<ITeamRepository, TeamEFRepository>();

builder.Services.AddControllersWithViews();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
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

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapControllerRoute(
    name: "game",
    pattern: "{controller=Game}/{action=List}/{team?}");

app.Run();

