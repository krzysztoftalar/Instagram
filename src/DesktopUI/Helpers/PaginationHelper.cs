using System;

namespace DesktopUI.Helpers
{
    public class PaginationHelper
    {
        public int Limit { get; set; } = 10;
        public int PageNumber { get; set; }
        public int ItemsCount { get; set; }
        public int Skip => PageNumber * Limit;
        public int TotalPages => (int) Math.Ceiling((double) ItemsCount / Limit);
    }
}