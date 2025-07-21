// using Microsoft.AspNetCore.Authentication.JwtBearer;
// using Microsoft.IdentityModel.Tokens;
// using MogoDbProductAPI.Data;
// using MogoDbProductAPI.Domain.Contracts;
// using MogoDbProductAPI.Extensions;
// using MogoDbProductAPI.Service;
// using Scalar.AspNetCore;
// using System.Text;

// var builder = WebApplication.CreateBuilder(args);

// // CORS
// builder.Services.ConfigureCors(builder.Configuration);

// // Controllers
// builder.Services.AddControllers();

// // Swagger (nếu bạn đang dùng Scalar.OpenAPI)
// builder.Services.AddOpenApi();

// // MongoDB config
// builder.Services.Configure<MongoDBSettings>(builder.Configuration.GetSection("MongoDB"));
// builder.Services.AddSingleton<AppDbContext>();

// // Service DI
// builder.Services.AddScoped<IProductService, ProductService>();
// builder.Services.AddScoped<IUserService, UserService>();

// // JWT Authentication
// builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
//     .AddJwtBearer(options =>
//     {
//         var key = Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]);
//         options.TokenValidationParameters = new TokenValidationParameters
//         {
//             ValidateIssuer = true,
//             ValidateAudience = true,
//             ValidateLifetime = true,
//             ValidateIssuerSigningKey = true,
//             ValidIssuer = builder.Configuration["Jwt:Issuer"],
//             ValidAudience = builder.Configuration["Jwt:Audience"],
//             IssuerSigningKey = new SymmetricSecurityKey(key)
//         };
//     });




// builder.Services.AddAuthorization();

// var app = builder.Build();

// app.Use(async (context, next) =>
// {
//     Console.WriteLine("Auth header: " + context.Request.Headers["Authorization"]);
//     await next();
// });

// // Middleware pipeline
// app.UseCors("CorsPolicy");

// if (app.Environment.IsDevelopment())
// {
//     app.MapOpenApi();
//     app.MapScalarApiReference(); // nếu bạn dùng Scalar
// }

// app.UseHttpsRedirection();

// app.UseAuthentication(); // 👈 thêm dòng này để dùng JWT
// app.UseAuthorization();

// app.MapControllers();

// app.Run();


using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using MogoDbProductAPI.Data;
using MogoDbProductAPI.Domain.Contracts;
using MogoDbProductAPI.Extensions;
using MogoDbProductAPI.Service;
using Scalar.AspNetCore;
using System.Text;
using System.Net;

var builder = WebApplication.CreateBuilder(args);

// CORS
builder.Services.ConfigureCors(builder.Configuration);

// Controllers
builder.Services.AddControllers();

// Swagger
builder.Services.AddOpenApi();

// MongoDB config
builder.Services.Configure<MongoDBSettings>(builder.Configuration.GetSection("MongoDB"));
builder.Services.AddSingleton<AppDbContext>();

// Service DI
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<IUserService, UserService>();

// JWT Authentication
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        var key = Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]);
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(key)
        };

        // Thêm phần xử lý khi authentication thất bại
        options.Events = new JwtBearerEvents
        {
            OnChallenge = async context =>
            {
                context.HandleResponse();
                context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                context.Response.ContentType = "application/json";
                await context.Response.WriteAsJsonAsync(new
                {
                    status = 401,
                    title = "Unauthorized",
                    detail = "You must be logged in to perform this action.",
                    type = "https://tools.ietf.org/html/rfc7235#section-3.1"
                });
            }
        };
    });

builder.Services.AddAuthorization();

var app = builder.Build();

app.Use(async (context, next) =>
{
    Console.WriteLine("Auth header: " + context.Request.Headers["Authorization"]);
    await next();
});

// Middleware pipeline
app.UseCors("CorsPolicy");

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();