using System;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Caliburn.Micro;
using DesktopUI.Helpers;

namespace DesktopUI.ViewModels.Base
{
    public abstract class BaseViewModel : Screen
    {
        protected static async Task RunCommand(Expression<Func<bool>> updatingFlag, Func<Task> action)
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