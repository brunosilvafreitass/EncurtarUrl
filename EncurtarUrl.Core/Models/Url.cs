using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EncurtarUrl.Core.Models
{
    public class Url
    {
        public int Id { get; set; }
        public string OriginalUrl { get; set; } = string.Empty;
        public string ShortenedCode { get; set; } = string.Empty;
        public string ShortenedUrl { get; set; } = string.Empty;
        public int ClickCount { get; set; } = 0;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow.AddHours(-3); // Adjust for UTC-3 timezone
    }
}