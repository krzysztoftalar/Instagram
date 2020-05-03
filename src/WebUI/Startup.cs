using Application;
using Application.Services.Profiles.Commands.Edit;
using Application.Services.User.Commands.Register;
using Application.Services.User.Queries.Login;
using FluentValidation.AspNetCore;
using Infrastructure.Photos;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Persistence;
using Persistence.Data;
using System.Text;
using WebUI.Configuration;
using WebUI.Middleware;

namespace WebUI
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers(options =>
                {
                    var policy = new AuthorizationPolicyBuilder().RequireAuthenticatedUser().Build();
                    options.Filters.Add(new AuthorizeFilter(policy));
                })
                .AddFluentValidation(opt =>
                {
                    opt.RegisterValidatorsFromAssemblyContaining<LoginUserQueryValidator>();
                    opt.RegisterValidatorsFromAssemblyContaining<RegisterUserCommandValidator>();
                    opt.RegisterValidatorsFromAssemblyContaining<EditProfileCommandValidator>();
                });

            services.AddApplication();
            services.AddInfrastructure();
            services.AddPersistence(Configuration);

            services.AddSwaggerDocumentation();

            services.AddHealthChecks()
                .AddDbContextCheck<ApplicationDbContext>();

            var key = new SymmetricSecurityKey(Encoding.UTF8
                .GetBytes(Configuration.GetValue<string>("TokenKey:SecurityKey")));

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

            services.Configure<CloudinarySettings>(Configuration.GetSection("Cloudinary"));
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseMiddleware<ErrorHandlingMiddleware>();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseSwaggerDocumentation();

            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
        }
    }
}