using AutoMapper;
using Caliburn.Micro;
using DesktopUI.Helpers;
using DesktopUI.Library.Api.Comment;
using DesktopUI.Library.Api.Profile;
using DesktopUI.Library.Api.User;
using DesktopUI.Library.Helpers;
using DesktopUI.Library.Models.DbModels;
using DesktopUI.Models;
using DesktopUI.ViewModels.Pages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using DesktopUI.Library.Models;
using Profile = DesktopUI.Library.Models.DbModels.Profile;

namespace DesktopUI
{
    public class Bootstrapper : BootstrapperBase
    {
        private readonly SimpleContainer _container = new SimpleContainer();

        public Bootstrapper()
        {
            ConventionManager.AddElementConvention<PasswordBox>(
                PasswordBoxHelper.BoundPasswordProperty,
                "Password",
                "PasswordChanged");

            Initialize();
        }

        public IMapper ConfigureAutoMapper()
        {
            var config = new MapperConfiguration(cfg =>
            {
                // Mapping Display model to api model 
                cfg.CreateMap<PhotoDisplayModel, Photo>();
                cfg.CreateMap<ProfileDisplayModel, Profile>();
                cfg.CreateMap<ProfileDisplayModel, ProfileFormValues>();
                cfg.CreateMap<CommentDisplayModel, Comment>();

                // Mapping Api model to display model 
                cfg.CreateMap<Photo, PhotoDisplayModel>();
                cfg.CreateMap<Profile, ProfileDisplayModel>();
                cfg.CreateMap<AuthenticatedUser, UserDisplayModel>();
                cfg.CreateMap<Comment, CommentDisplayModel>();
            });

            return config.CreateMapper();
        }

        protected override void Configure()
        {
            _container.Instance(ConfigureAutoMapper());

            // Api call methods
            _container.Instance(_container)
                .PerRequest<IUserEndpoint, UserEndpoint>()
                .PerRequest<IProfileEndpoint, ProfileEndpoint>()
                .PerRequest<ICommentEndpoint, CommentEndpoint>();

            // Caliburn.Micro methods
            _container
                .Singleton<IWindowManager, WindowManager>()
                .Singleton<IEventAggregator, EventAggregator>();

            // Api settings methods
            _container
                .Singleton<IApiHelper, ApiHelper>()
                .Singleton<IChatHelper, ChatHelper>();

            // Display models
            _container
                .Singleton<IAuthenticatedUser, UserDisplayModel>()
                .Singleton<IProfile, ProfileDisplayModel>()
                .Singleton<IPhoto, PhotoDisplayModel>();

            GetType().Assembly.GetTypes()
                .Where(type => type.IsClass)
                .Where(type => type.Name.EndsWith("ViewModel"))
                .ToList()
                .ForEach(viewModelType => _container.RegisterPerRequest(
                    viewModelType, viewModelType.ToString(), viewModelType));
        }

        protected override void OnStartup(object sender, StartupEventArgs e)
        {
            DisplayRootViewFor<ShellViewModel>();
        }

        protected override object GetInstance(Type service, string key)
        {
            return _container.GetInstance(service, key);
        }

        protected override IEnumerable<object> GetAllInstances(Type service)
        {
            return _container.GetAllInstances(service);
        }

        protected override void BuildUp(object instance)
        {
            _container.BuildUp(instance);
        }
    }
}