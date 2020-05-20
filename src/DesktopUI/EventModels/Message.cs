using System;

namespace DesktopUI.EventModels
{
    public class Message : IMessage
    {
        public event EventHandler<bool> ProfilePage;

        public void OnProfilePage(bool e)
        {
            ProfilePage?.Invoke(this, e);
        }
    }
}
