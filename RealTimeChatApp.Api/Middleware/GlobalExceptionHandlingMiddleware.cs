using FluentValidation;
using Microsoft.AspNetCore.SignalR;
using System.Net;
using System.Text.Json;

namespace RealTimeChatApp.Api.Middleware
{
    public class GlobalExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        public GlobalExceptionHandlingMiddleware(RequestDelegate next)
        {
            _next = next;
        }
        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex);
            }
        }
        private async Task HandleExceptionAsync(HttpContext context, Exception ex)
        {
            context.Response.ContentType = "application/json";
            var response = new ErrorResponse();
            // FluentValidation Exception
            if (ex is HubException hubEx)
            {
                context.Response.StatusCode = (int)HttpStatusCode.BadRequest;

                response.Message = hubEx.Message;
                response.Errors = new List<string> { hubEx.Message };
            }
           else  if (ex is ValidationException validationEx)
            {
                context.Response.StatusCode = (int)HttpStatusCode.BadRequest;

                response.Message = "Validation failed.";
                response.Errors = validationEx.Errors
                    .Select(e => e.ErrorMessage)
                    .ToList();
            }
            else
            {

                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

                response.Message = ex.Message;
                response.Errors = new List<string> { "An unexpected error occurred." };
            }

            var json = JsonSerializer.Serialize(response, new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            });

            await context.Response.WriteAsync(json);
        }
    }
    public class ErrorResponse
    {
        public string Message { get; set; }
        public List<string>? Errors { get; set; }
    }
}
