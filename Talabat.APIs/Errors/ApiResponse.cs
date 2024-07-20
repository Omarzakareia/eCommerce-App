
namespace Talabat.APIs.Errors
{
    public class ApiResponse
    {
        public int? StatusCode { get; set; }
        public string? Message { get; set; }
        public ApiResponse(int statusCode , string? message = null)
        {
            StatusCode = statusCode;
            Message = message ?? GetDefaultMessageForStatusCode(StatusCode);
        }

        private string? GetDefaultMessageForStatusCode(int? statusCode)
        {
            // 500 => Internal Server Error
            // 400 => Bad Request
            // 401 => Unautherized
            // 404 => Not Found

            // switch expression => C# 7 
            return StatusCode switch
            {
                400 => "Bad Request",
                401 => "You Are Not Autherized",
                404 => "Resource Not Found",
                500 => "Internal Server Error",
                _ => null
            };
        }
    }
}
