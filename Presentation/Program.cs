using Applications;
using Infrastructure;
using DotNetEnv;
using Microsoft.OpenApi.Models;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

Env.Load();
builder.Configuration.AddEnvironmentVariables();
builder.Services.AddControllers();

// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo 
    { 
        Title = "University System API", 
        Version = "v1",
        Description = "A comprehensive university management system API"
    });
    
    // Add JWT authentication to OpenAPI spec
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Please Enter 'Bearer' [space] and then your token in the text input below.",
        Name = "Authorization",
        Type = SecuritySchemeType.Http, 
        Scheme = "bearer",
        BearerFormat = "JWT"
    });
    
    // Apply security requirement globally to all operations
    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                },
                In = ParameterLocation.Header
            },
            new List<string>()
        }
    });
});

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
    app.UseSwagger();
    app.UseSwaggerUI();
    
    app.MapScalarApiReference(options =>
    {
        options
            .WithTitle("University System API")
            .WithTheme(ScalarTheme.Kepler)
            .WithDefaultHttpClient(ScalarTarget.CSharp, ScalarClient.HttpClient)
            .WithOpenApiRoutePattern("/swagger/{documentName}/swagger.json"); // Make sure Scalar uses the right endpoint
    });
}

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
