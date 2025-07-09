using Applications;
using Infrastructure;
using DotNetEnv;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

Env.Load();
builder.Configuration.AddEnvironmentVariables();
builder.Services.AddControllers();

// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.AddApplicationServices();
builder.Services.AddInfrastructureServices(builder.Configuration);

var cors = "AllowAll";
//var _ourDomain = "www.ourdomain1.com";
//var _ourDomain2 = "www.ourdomain2.com";
builder.Services.AddCors(options => options.AddPolicy(name: cors, policy =>
{
    // policy.WithOrigins(_ourDomain1, _ourDomain2);
    policy.AllowAnyOrigin();
    policy.AllowAnyMethod();
    policy.AllowAnyHeader();
    // policy.AllowCredentials(); // Allow sending cookies/auth headers
}));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference(options =>
    {
        options.WithTitle("University System API")
            .WithTheme(ScalarTheme.Purple);
    });
}

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
