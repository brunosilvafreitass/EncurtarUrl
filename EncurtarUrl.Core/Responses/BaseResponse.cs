using System.Text.Json.Serialization;

namespace EncurtarUrl.Core.Responses
{
    public class BaseResponse<TData>
    {
        private readonly int _code;

        [JsonConstructor]
        public BaseResponse() => _code = 200;

        public BaseResponse(TData? data, int code = 200, string? message = null)
        {
            Data = data;
            _code = code;
            Message = message;
        }

        public TData? Data { get; set; }

        public string? Message { get; set; }

        [JsonIgnore]
        public bool IsSuccess => _code >= 200 && _code < 299;
    }
}