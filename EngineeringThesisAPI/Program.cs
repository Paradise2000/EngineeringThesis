using EngineeringThesisAPI;
using EngineeringThesisAPI.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//database setup
builder.Services.AddDbContext<EngineeringThesisDbContext>
    (options => options.UseSqlServer(builder.Configuration.GetConnectionString("Database")));

//Auth setup
builder.Services.AddScoped<IPasswordHasher<User>, PasswordHasher<User>>();

var AuthSettings = new AuthenticationSettings();
builder.Configuration.GetSection("Authentication").Bind(AuthSettings);
builder.Services.AddAuthentication(option =>
{
    option.DefaultAuthenticateScheme = "Bearer";
    option.DefaultScheme = "Bearer";
    option.DefaultChallengeScheme = "Bearer";
}).AddJwtBearer(con =>
{
    con.RequireHttpsMetadata = false;
    con.SaveToken = true;
    con.TokenValidationParameters = new TokenValidationParameters
    {
        ValidIssuer = AuthSettings.JwtIssuer,
        ValidAudience = AuthSettings.JwtIssuer,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(AuthSettings.JwtKey)),
    };
});
builder.Services.AddSingleton(AuthSettings);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthentication();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
