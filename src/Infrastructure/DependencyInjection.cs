using Application.Interfaces;
using Infrastructure.Email;
using Infrastructure.Email.SendGrid;
using Infrastructure.Photos;
using Infrastructure.Security;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services)
        {
            services.AddScoped<IJwtGenerator, JwtGenerator>();
            services.AddScoped<IUserAccessor, UserAccessor>();
            services.AddScoped<IPhotoAccessor, PhotoAccessor>();
            services.AddScoped<IEmailAccessor, EmailAccessor>();

            services.AddTransient<ISendGridEmailSender, SendGridEmailSender>();
            services.AddTransient<IEmailTemplateSender, EmailTemplateSender>();

            return services;
        }
    }
}