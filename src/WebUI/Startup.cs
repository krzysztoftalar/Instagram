using Application;
using Application.Interfaces;
using FluentValidation.AspNetCore;
using Infrastructure;
using Infrastructure.Photos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Persistence;
using WebUI.Configurations;
using WebUI.Middleware;
using WebUI.SignalR;

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
                .AddFluentValidation(options =>
                {
                    options.RegisterValidatorsFromAssemblyContaining<IApplicationDbContext>();
                });

            services.AddHttpContextAccessor();

            services.AddApplication();
            services.AddInfrastructure();
            services.AddPersistence(Configuration);
            services.AddSignalR();

            services.AddSwaggerDocumentation();
            services.AddJwtIdentity(Configuration.GetSection("JwtConfiguration"));

            services.Configure<CloudinarySettings>(Configuration.GetSection("Cloudinary"));
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseMiddleware<ErrorHandlingMiddleware>();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseSwaggerDocumentation();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapHub<ChatHub>("/chat");
            });
        }
    }
}