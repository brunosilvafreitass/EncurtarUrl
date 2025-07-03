using System.ComponentModel.DataAnnotations;

namespace EncurtarUrl.Core.Requests
{
    public class ShortenedUrlRequest : BaseRequest
    {
        [Required(ErrorMessage = "Url não pode ser vazia.")]
        public string OriginalUrl { get; set; } = string.Empty;
    }
}