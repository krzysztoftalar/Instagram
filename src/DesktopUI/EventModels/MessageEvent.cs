namespace DesktopUI.EventModels
{
    public class MessageEvent
    {
        public string Username { get; set; }

        public string Predicate { get; set; }

        public bool HandleGetNextComments { get; set; }
        public bool HandleGetNextPhotos { get; set; }
    }
}
