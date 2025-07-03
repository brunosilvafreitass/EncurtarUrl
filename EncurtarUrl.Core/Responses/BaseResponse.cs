using System.Text.Json.Serialization;

namespace EncurtarUrl.Core.Responses
{
    public class BaseResponse<TData>
    {
        private readonly int _code;

        [JsonConstructor]
        public BaseResponse() => _code = 200;
    }
}