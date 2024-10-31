using Microsoft.EntityFrameworkCore;
using ShipFast.Authentication.DataAccess;
using ShipFast.Authentication.DataAccess.Models;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

ConfigureServices(
    builder.Services,
    builder.Configuration
);

var app = builder.Build();

// POST Authenticate User
app.MapPost("/authenticate", async (AuthDbContext authDbContext, User loginUser) =>
{
    var user = await authDbContext.Users
        .FirstOrDefaultAsync(u => u.Email == loginUser.Email && u.Password == loginUser.Password);

    if (user == null)
    {
        return Results.Unauthorized();
    }

    var token = GenerateJwtToken(user);
    return Results.Ok(new { token });
});

app.Run();

void ConfigureServices(IServiceCollection services, ConfigurationManager configurationManager)
{
    services.AddDbContext<AuthDbContext>(opts =>
        opts.UseNpgsql(configurationManager.GetConnectionString("AuthDb")),
        ServiceLifetime.Transient);
    services.AddEndpointsApiExplorer();
    services.AddSwaggerGen();
}

string GenerateJwtToken(User user)
{
    var key = Encoding.ASCII.GetBytes("YourSecretKeyHere");
    var tokenHandler = new JwtSecurityTokenHandler();
    var tokenDescriptor = new SecurityTokenDescriptor
    {
        Subject = new ClaimsIdentity(new Claim[]
        {
            new Claim(ClaimTypes.Name, user.Id.ToString())
        }),
        Expires = DateTime.UtcNow.AddHours(1),
        SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
    };
    var token = tokenHandler.CreateToken(tokenDescriptor);
    return tokenHandler.WriteToken(token);
}
