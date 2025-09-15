using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using EVConnectService.Models;
using Microsoft.IdentityModel.Tokens;

public class TokenService
{
    private readonly IConfiguration _configuration;

    public TokenService(IConfiguration configuration)
    {
        _configuration = configuration;
    }


    //  Generates a JWT token for the given user.
    public object GenerateToken(User user)
    {
        // Create a token handler to manage JWTs
        var tokenHandler = new JwtSecurityTokenHandler();
        // Retrieve the secret key from the application's configuration and convert it to a byte array.
        var keyString = _configuration["Jwt:Key"]
                ?? throw new InvalidOperationException("JWT Key is missing in configuration.");

        var key = Encoding.UTF8.GetBytes(keyString);

        // Define the token's properties using a SecurityTokenDescriptor.
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            // Add a ClaimsIdentity, which holds the user's claims.
            // Claims are pieces of information about the user, like their name, NIC, and email.
            Subject = new ClaimsIdentity(new[]
            {
                new Claim(ClaimTypes.Name, user.Name),
                new Claim("NIC", user.NIC),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Role, user.Role)
            }),
            // Set the token's expiration time to 2 hours from the current UTC time.
            Expires = DateTime.UtcNow.AddHours(2),
            // Define the token's issuer (the party that created the token) from configuration.
            Issuer = _configuration["Jwt:Issuer"],
            // Define the token's audience (the intended recipient of the token) from configuration.
            Audience = _configuration["Jwt:Audience"],
            // Specify the signing credentials, which include the secret key and the algorithm (HMAC SHA-256).
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };

        // Create the JWT using the token descriptor.
        var token = tokenHandler.CreateToken(tokenDescriptor);

        return new
        {
            // Serialize the token object into a string.
            Token = tokenHandler.WriteToken(token)
        };
    }
}