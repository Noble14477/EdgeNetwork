using System;
using System.Collections.Generic;
using System.Text;

namespace EdgeNetworkApplication.Common
{
    public class ApiResponse<T>
    {
        public bool Succeeded { get; set; }
        public string Message { get; set; }
        public T? Data { get; set; }

        private ApiResponse() { }

        public static ApiResponse<T> Success(T? data, string message = "Request successful.")
        {
            return new ApiResponse<T>
            {
                Succeeded = true,
                Message = message,
                Data = data
            };
        }

        public static ApiResponse<T> Failure(string message = "Request failed.")
        {
            return new ApiResponse<T>
            {
                Succeeded = false,
                Message = message,
                Data = default
            };
        }
    }
}
