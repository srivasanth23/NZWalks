using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using NZWalks.API.Data;
using NZWalks.API.Mappings;
using NZWalks.API.Middlewares;
using NZWalks.API.Repositories;
using Serilog;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Adding serilog configuration
var logger = new LoggerConfiguration()
    .WriteTo.Console()                       // This will get the logs in Console
    .WriteTo.File("Logs/NZWalks_Log.txt", rollingInterval: RollingInterval.Day)     // This will get us logs us in File (per day-wise)
    .MinimumLevel.Information()             // For normal log // .Warning() ---> For warning logs // .Error() --> For error logs
    .CreateLogger();

builder.Logging.ClearProviders();
builder.Logging.AddSerilog(logger);


builder.Services.AddControllers();
builder.Services.AddHttpContextAccessor(); // We are injecting this so that we can consume it in our application


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
builder.Services.AddScoped<IImageRepo, LocalImageRepo>();

builder.Services.AddAutoMapper(typeof(AutoMapperProfiles).Assembly);

// Identity Solution - Setting up ASP.NET Core Identity services
// Registers the Identity core services for user management like CRUD operations
builder.Services.AddIdentityCore<IdentityUser>()
    .AddRoles<IdentityRole>()           // Enables role-based authorization
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
app.UseMiddleware<ExceptionHandellerMiddleware>();
app.UseHttpsRedirection();

app.UseAuthentication(); // We are adding because we need to to authenticate before authorization
app.UseAuthorization();

// Serving Static File 
app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), "Images")),
    RequestPath = "/Images"
    // https://localhost:1234/Images
});

app.MapControllers();

app.Run();
