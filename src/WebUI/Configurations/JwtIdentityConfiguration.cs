using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace WebUI.Configurations
{
    public static class JwtIdentityConfiguration
    {
        public static IServiceCollection AddJwtIdentity(this IServiceCollection services, IConfigurationSection jwtConfiguration)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtConfiguration["SecurityKey"]));

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
              .AddJwtBearer(options =>
              {
                  options.TokenValidationParameters = new TokenValidationParameters
                  {
                      ValidateIssuerSigningKey = true,
                      IssuerSigningKey = key,
                      ValidateAudience = false,
                      ValidateIssuer = false
                  };
              });

            return services;
        }
    }
}
