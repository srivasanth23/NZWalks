using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using NZWalks.API.Data;
using NZWalks.API.Mappings;
using NZWalks.API.Repositories;
using System.Text;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "NZ Walks API",
        Version = "v1"
    });
    options.AddSecurityDefinition(JwtBearerDefaults.AuthenticationScheme, new OpenApiSecurityScheme
    {
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = JwtBearerDefaults.AuthenticationScheme
    });
    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = JwtBearerDefaults.AuthenticationScheme
                },
                Scheme = "OAuth2",
                Name = JwtBearerDefaults.AuthenticationScheme,
                In = ParameterLocation.Header
            },
            new List<string>()
        }
    });
});

// Database DI
builder.Services.AddDbContext<NZWalksDbContext>(options =>
options.UseSqlServer(builder.Configuration.GetConnectionString("NZWalksConnectionStrings")));

builder.Services.AddDbContext<NZWalksAuthDbContext>(options =>
options.UseSqlServer(builder.Configuration.GetConnectionString("NZWalksAuthConnectionString")));

builder.Services.AddScoped<IRegionRepo, SQLRegionRepo>();
builder.Services.AddScoped<IWalkRepo, SQLWalkRepository>();
builder.Services.AddScoped<ITokenRepositary, TokenRepo>();

builder.Services.AddAutoMapper(typeof(AutoMapperProfiles).Assembly);

// Identity Solution - Setting up ASP.NET Core Identity services
// Registers the Identity core services for user management like CRUD operations
builder.Services.AddIdentityCore<IdentityUser>()
    .AddRoles<IdentityRole>()   // Enables role-based authorization
                                // Adds role management support (for assigning roles to users)
    .AddTokenProvider<DataProtectorTokenProvider<IdentityUser>>("NZWalks")   // Adds a token system for generating secure tokens for tasks like email verification or password reset.
    .AddEntityFrameworkStores<NZWalksAuthDbContext>()     // Uses Entity Framework to store Identity data (Users, Roles, etc.) in the database
    .AddDefaultTokenProviders();        // Adds default token providers for operations like password reset, email confirmation, etc.


// Configure Identity Options
builder.Services.Configure<IdentityOptions>(options =>
{
    // Password settings
    options.Password.RequireDigit = false;
    options.Password.RequireLowercase = false;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = false;
    options.Password.RequiredLength = 6;
    options.Password.RequiredUniqueChars = 1;
});


// Adding JWT Authentication
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
options.TokenValidationParameters = new TokenValidationParameters
{
    AuthenticationType = "Jwt",
    ValidateIssuer = true,
    ValidateAudience = true,
    ValidateLifetime = true,
    ValidateIssuerSigningKey = true,
    ValidIssuer = builder.Configuration["JWT:Issuer"],
    //ValidAudience = builder.Configuration["JWT.Audience"],
    ValidAudiences = new[] { builder.Configuration["JWT:Audience"] },
    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JWT:Key"]))
});



var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication(); // We are adding because we need to to authenticate before authorization
app.UseAuthorization();

app.MapControllers();

app.Run();
