using System.Text;
using Core.Identity;
using Core.IRepositories;
using Core.IServices;
using Ecommerce.Middleware;
using Infrastructure.Data;
using Infrastructure.Identity;
using Infrastructure.Repositories;
using Infrastructure.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using StackExchange.Redis;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddDbContext<StoreContext>(options => {
    options.UseSqlite(builder.Configuration.GetConnectionString("Default"));
});

builder.Services.AddDbContext<AppIdentityDbContext>(options => {
    options.UseSqlite(builder.Configuration.GetConnectionString("Identity"));
});

builder.Services.AddSingleton<IConnectionMultiplexer>( c => {
    var options = ConfigurationOptions.Parse(builder.Configuration.GetConnectionString("Redis"));
    return ConnectionMultiplexer.Connect(options);
});

builder.Services.AddIdentityCore<AppUser>(options => {

})
    .AddEntityFrameworkStores<AppIdentityDbContext>()
    .AddSignInManager<SignInManager<AppUser>>();

var jwt = builder.Configuration.GetSection("Token");

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options => {
        options.TokenValidationParameters = new TokenValidationParameters {
            ValidateIssuerSigningKey = true,
            ValidateIssuer = true,
            ValidateAudience = false,
            ValidIssuer = jwt["Issuer"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwt["Key"]))
        };
    });

builder.Services.AddAuthorization();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
builder.Services.AddScoped<IBasketRepository, BasketRepository>();
builder.Services.AddScoped<ITokenService, TokenService>();
builder.Services.AddScoped<IOrderService, OrderService>();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

builder.Services.AddCors(option => {
    option.AddPolicy("CorsPolicy", policy => {
        policy.AllowAnyHeader().AllowAnyMethod().WithOrigins("http://localhost:4200");
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Testing API V1");
    });
}

app.UseMiddleware<ExceptionHandlerMiddleware>();

app.UseStatusCodePagesWithReExecute("/errors/{0}");

app.UseStaticFiles();

app.UseCors("CorsPolicy");

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseRouting();

app.UseAuthorization();

app.MapControllers();

using var scope = app.Services.CreateScope();
var context = scope.ServiceProvider.GetRequiredService<StoreContext>();
var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();

var identityContext = scope.ServiceProvider.GetRequiredService<AppIdentityDbContext>();
var userManager = scope.ServiceProvider.GetRequiredService<UserManager<AppUser>>();

try
{
    await context.Database.MigrateAsync();
    await StoreContextSeed.SeedingData(context);

    await identityContext.Database.MigrateAsync();
    await AppIdentitySeedingData.SeedUserAsync(userManager);
}
catch (Exception ex)
{
    logger.LogError(ex, "Error occured during creating migration.");
}

app.Run();
