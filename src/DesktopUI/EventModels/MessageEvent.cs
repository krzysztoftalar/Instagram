namespace DesktopUI.EventModels
{
    public class MessageEvent
    {
        public string Username { get; set; }
        public string Predicate { get; set; }

        public string DisplayName { get; set; }
        public string Bio { get; set; }
        public bool IsEditMode { get; set; }
    }
}
