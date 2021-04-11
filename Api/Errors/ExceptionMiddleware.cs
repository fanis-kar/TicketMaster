using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using TicketMaster.Business.Exceptions;

namespace TicketMaster.Api.Errors
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionMiddleware> _logger;

        public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
        {
            _logger = logger;
            _next = next;
        }

        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong: {ex}");
                await HandleExceptionAsync(httpContext, ex);
            }
        }

        private Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/json";
            var details = new ErrorDetails()
            {
                Message = exception.Message
            };


            if (exception.GetType() == typeof(ItemNotFoundException) ||
                exception.GetType() == typeof(ShowNotFoundException))
            {
                details.StatusCode = (int) HttpStatusCode.NotFound;
            }
            else if (exception.GetType() == typeof(InvalidTicketPriceException) ||
                exception.GetType() == typeof(InvalidTicketQtyException) ||
                exception.GetType() == typeof(SoldOutException) ||
                exception.GetType() == typeof(ReservationFailedException))
            {
                details.StatusCode = (int)HttpStatusCode.RequestedRangeNotSatisfiable; ;
            }
            else
            {
                details.StatusCode = (int)HttpStatusCode.InternalServerError;
                details.Message = "Internal Server Error from the custom middleware.";
            }
            context.Response.StatusCode = details.StatusCode;
            return context.Response.WriteAsync(details.ToString());
        }
    }
}
