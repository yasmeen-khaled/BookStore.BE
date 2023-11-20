using BookStore.BLL;
using BookStore.DAL;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
#region Cors

var corsPolicy = "AllowAll";
builder.Services.AddCors(options =>
{
    options.AddPolicy(corsPolicy, p => p.AllowAnyOrigin()
    .AllowAnyHeader()
    .AllowAnyMethod());
});

#endregion
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

#region Database

var connectionString = builder.Configuration.GetConnectionString("SystemCon");
builder.Services.AddDbContext<SystemContext>(options
    => options.UseSqlServer(connectionString));

#endregion
#region Repos

builder.Services.AddScoped<IBookRepository, BookRepository>();

#endregion
#region Managers

builder.Services.AddScoped<IBookManager, BookManager>();

#endregion
#region Identity Managers

builder.Services.AddIdentity<IdentityUser, IdentityRole>(options =>
{
    options.Password.RequireUppercase = false;
    options.Password.RequireLowercase = false;
    options.Password.RequireNonAlphanumeric = false;

    options.User.RequireUniqueEmail = false;
})
    .AddEntityFrameworkStores<SystemContext>();

#endregion
#region Authentication

builder.Services.AddAuthentication(options =>
{
    //Used Authentication Scheme
    options.DefaultAuthenticateScheme = "CoolAuthentication";

    //Used Challenge Authentication Scheme
    options.DefaultChallengeScheme = "CoolAuthentication";
})
    .AddJwtBearer("CoolAuthentication", options =>
    {
        var secretKeyString = builder.Configuration.GetValue<string>("SecretKey") ?? string.Empty;
        var secretKeyInBytes = Encoding.ASCII.GetBytes(secretKeyString);
        var secretKey = new SymmetricSecurityKey(secretKeyInBytes);

        options.TokenValidationParameters = new TokenValidationParameters
        {
            IssuerSigningKey = secretKey,
            ValidateIssuer = false,
            ValidateAudience = false
        };
    });

#endregion

#region Authorization

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("allowAdmins", policy => policy
        .RequireClaim(ClaimTypes.Role, "admin")
        .RequireClaim(ClaimTypes.NameIdentifier));

});

#endregion

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors(corsPolicy);

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
