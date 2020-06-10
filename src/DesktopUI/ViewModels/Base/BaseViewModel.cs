using Caliburn.Micro;
using DesktopUI.Helpers;
using System;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace DesktopUI.ViewModels.Base
{
    public class BaseViewModel : Screen
    {
        public async Task RunCommand(Expression<Func<bool>> updatingFlag, Func<Task> action)
        {
            if (updatingFlag.GetPropertyValue()) return;

            updatingFlag.SetPropertyValue(true);

            try
            {
                await action();
            }
            finally
            {
                updatingFlag.SetPropertyValue(false);
            }
        }
    }
}