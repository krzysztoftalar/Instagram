using Caliburn.Micro;
using DesktopUI.Helpers;
using DesktopUI.Library.Api.Comment;
using DesktopUI.Library.Api.Profile;
using DesktopUI.Library.Api.User;
using DesktopUI.Library.Helpers;
using DesktopUI.Library.Models;
using DesktopUI.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using DesktopUI.Library.Models.DbModels;

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

        protected override void Configure()
        {
            _container.Instance(_container)
                .PerRequest<IUserEndpoint, UserEndpoint>()
                .PerRequest<IProfileEndpoint, ProfileEndpoint>()
                .PerRequest<ICommentEndpoint, CommentEndpoint>();

            _container
                .Singleton<IWindowManager, WindowManager>()
                .Singleton<IEventAggregator, EventAggregator>()
                .Singleton<IApiHelper, ApiHelper>()
                .Singleton<IChatHelper, ChatHelper>()
                .Singleton<IAuthenticatedUser, AuthenticatedUser>()
                .Singleton<IProfile, Profile>()
                .Singleton<IPhoto, Photo>();
               
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