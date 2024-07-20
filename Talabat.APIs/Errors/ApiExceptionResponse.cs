namespace Talabat.APIs.Errors
{
    public class ApiExceptionResponse : ApiResponse
    {
        public ApiExceptionResponse(int statuscode , string? Message = null , string? details = null) :base(statuscode , Message)
        {
            Details = details;
        }
        public string? Details { get; set; }
    }
}
