using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineCasino.Application.DTOs
{
    public class Response<T>
    {
        public Response()
        {
            Success = false;
            Message = "Something went wrong";
        }

        public Response(bool success, string message)
        {
            Success = success; 
            Message = message;
        }

        public Response(T data)
        {
            Success = true;
            Message = "All done";
            Data = data;
        }

        public bool Success { get; set; }

        public string Message { get; set; }

        public T? Data { get; set; }
    }

}
