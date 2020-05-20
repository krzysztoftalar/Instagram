using System;

namespace DesktopUI.EventModels
{
    public interface IMessage
    {
        event EventHandler<bool> ProfilePage;
        void OnProfilePage(bool e);
    }
}
