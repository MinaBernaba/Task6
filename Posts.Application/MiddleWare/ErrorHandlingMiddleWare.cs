using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using PostsProject.Application.ResponseBase;
using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Text.Json;

namespace PostsProject.Core.MiddleWare
{
    public class ErrorHandlerMiddleware(RequestDelegate next)
    {
        private readonly RequestDelegate _next = next;

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception error)
            {
                var contextResponse = context.Response;
                contextResponse.ContentType = "application/json";

                var response = new Response<string>() { Succeeded = false, Message = error.Message };

                switch (error)
                {
                    case UnauthorizedAccessException e:
                        response.StatusCode = HttpStatusCode.Unauthorized;
                        contextResponse.StatusCode = (int)HttpStatusCode.Unauthorized;
                        break;

                    case ValidationException e:
                        response.StatusCode = HttpStatusCode.UnprocessableEntity;
                        contextResponse.StatusCode = (int)HttpStatusCode.UnprocessableEntity;
                        break;

                    case KeyNotFoundException e:
                        response.StatusCode = HttpStatusCode.NotFound;
                        contextResponse.StatusCode = (int)HttpStatusCode.NotFound;
                        break;

                    case DbUpdateException:
                        response.StatusCode = HttpStatusCode.BadRequest;
                        contextResponse.StatusCode = (int)HttpStatusCode.BadRequest;
                        break;

                    case Exception e:
                        response.Message += e.InnerException == null ? "" : "\n" + e.InnerException.Message;
                        response.StatusCode = HttpStatusCode.BadRequest;
                        contextResponse.StatusCode = (int)HttpStatusCode.BadRequest;
                        break;

                    default:
                        // unhandled error
                        response.StatusCode = HttpStatusCode.InternalServerError;
                        contextResponse.StatusCode = (int)HttpStatusCode.InternalServerError;
                        break;
                }
                var result = JsonSerializer.Serialize(response);

                await contextResponse.WriteAsync(result);
            }
        }
    }
}
