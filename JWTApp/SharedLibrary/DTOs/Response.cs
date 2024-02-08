using System.Text.Json.Serialization;

namespace JWTApp.SharedLibrary.DTOs
{
    public class Response<T> where T : class
    {
        public T Data { get; private set; }
        public int StatusCode { get; private set; }
        [JsonIgnore]
        public bool IsSuccessful { get; private set; } //method çağrıldığında gösterilmesin
        public ErrorDto Error { get; private set; }
        public static Response<T> Success(T data, int statusCode)
        {

            return new Response<T> { Data = data, StatusCode = statusCode, IsSuccessful = true };
        }
        public static Response<T> Success(int statusCode)
        {
            return new Response<T> { Data = default, StatusCode = statusCode, IsSuccessful = true };
        }
        public static Response<T> Fail(ErrorDto errorDto, int statusCode)
        {
            return new Response<T> { Error = errorDto, StatusCode = statusCode, IsSuccessful = false };
        }
        public static Response<T> Fail(string errorMessage, int statusCode, bool isShow)
        {
            var errrorDto = new ErrorDto(errorMessage, isShow);
            return new Response<T>
            {
                Error = errrorDto,
                StatusCode = statusCode,

                IsSuccessful = false

            };
        }
    }
}
