using EP.Services.Enums;

namespace EP.Services.Blobs
{
    public sealed class BlobDeleteResult
    {
        public BlobDeleteResult(
            string id,
            ApiStatusCode statusCode = ApiStatusCode.OK,
            string message = null)
        {
            Id = id;
            StatusCode = (int)statusCode;
            Message = message;
        }

        public string Id { get; }

        public int StatusCode { get; }

        public string Message { get; }

        public bool IsSuccess()
        {
            return StatusCode == (int)ApiStatusCode.OK;
        }
    }
}