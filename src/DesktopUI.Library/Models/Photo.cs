﻿namespace DesktopUI.Library.Models
{
    public class Photo : IPhoto
    {
        public string Id { get; set; }
        public string Url { get; set; }
        public bool IsMain { get; set; }
    }
}
