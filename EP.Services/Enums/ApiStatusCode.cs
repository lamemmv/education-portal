using System.Net;

namespace EP.Services.Enums
{
    public enum ApiStatusCode : int
    {
        OK = HttpStatusCode.OK,
        Created = HttpStatusCode.Created,
        NoContent = HttpStatusCode.NoContent,
        BadRequest = HttpStatusCode.BadRequest,
        Unauthorized = HttpStatusCode.Unauthorized,
        Forbidden = HttpStatusCode.Forbidden,
        NotFound = HttpStatusCode.NotFound,
        InternalServerError = HttpStatusCode.InternalServerError,
        ServiceUnavailable = HttpStatusCode.ServiceUnavailable,
        // Blob.
        Blob_InvalidParent = 1001,
        Blob_DuplicatedName,
        Blob_HasChildren,
        Blob_SystemDirectory,
        Blob_InvalidMIMEType,
    }
}
