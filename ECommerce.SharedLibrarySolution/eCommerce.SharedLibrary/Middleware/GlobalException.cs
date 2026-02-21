using eCommerce.SharedLibrary.Logging;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Text.Json;

namespace eCommerce.SharedLibrary.Middleware
{
    public class GlobalException(RequestDelegate next)
    {
        public async Task InvokeAsync(HttpContext context)
        {
            string message = "Internal server error occurred. Kindly Try again";
            int statusCode = (int)HttpStatusCode.InternalServerError;
            string title = "Internal Server Error";
            try
            {
                await next(context);

                if(context.Response.StatusCode == StatusCodes.Status429TooManyRequests)
                {
                    title = "Warning";
                    message = "Too many requests. Please try again later.";
                    statusCode = (int)HttpStatusCode.TooManyRequests;
                    await ModifyHeader(context, title, message, statusCode);
                }
                if(context.Response.StatusCode == StatusCodes.Status401Unauthorized)
                {
                    title = "Alert";
                    message = "You are not authorized to access this resource.";
                    statusCode = (int)HttpStatusCode.Unauthorized;
                    await ModifyHeader(context, title, message, statusCode);
                }
                if(context.Response.StatusCode == StatusCodes.Status403Forbidden)
                {
                    title = "Out of Access";
                    message = "You are not allowed to access this resource.";
                    statusCode = (int)HttpStatusCode.Forbidden;
                    await ModifyHeader(context, title, message, statusCode);
                }
            }
            catch (Exception ex)
            {
                LogException.LogExceptions(ex);

                if(ex is TaskCanceledException || ex is TimeoutException)
                {
                    title = "Request Timeout";
                    message = "The request has timed out. Please try again later.";
                    statusCode = (int)HttpStatusCode.RequestTimeout;
                }
                await ModifyHeader(context, title, message, statusCode);
            }
        }

        private async Task ModifyHeader(HttpContext context, string title, string message, int statusCode)
        {
            context.Response.ContentType = "application/json";
            await context.Response.WriteAsync(JsonSerializer.Serialize(new ProblemDetails
            {
                Title = title,
                Detail = message,
                Status = statusCode
            }), CancellationToken.None);
            return;
        }
    }
}
