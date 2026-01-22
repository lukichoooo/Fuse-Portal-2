using System.ComponentModel.DataAnnotations;
using FusePortal.Domain.SeedWork;

namespace FusePortal.Api.Filters
{
    public sealed class ExceptionHandlingMiddleware // TODO: register
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionHandlingMiddleware> _logger;

        public ExceptionHandlingMiddleware(
            RequestDelegate next,
            ILogger<ExceptionHandlingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unhandled exception");

                await HandleAsync(context, ex);
            }
        }

        private static Task HandleAsync(HttpContext context, Exception ex)
        {
            var (status, error) = ex switch
            {
                DomainException clientEx => (
                    StatusCodes.Status400BadRequest,
                    new ApiError(clientEx.Message, clientEx.Message)
                ),

                ValidationException => (
                    StatusCodes.Status400BadRequest,
                    new ApiError("validation_error", ex.Message)
                ),

                _ => (
                    StatusCodes.Status500InternalServerError,
                    new ApiError("internal_error", "Something went wrong.")
                )
            };

            context.Response.StatusCode = status;
            context.Response.ContentType = "application/json";

            return context.Response.WriteAsJsonAsync(
                new ApiErrorResponse(error)
            );
        }
    }

}
