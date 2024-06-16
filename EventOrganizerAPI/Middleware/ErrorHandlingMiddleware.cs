using EventOrganizerAPI.Exceptions;

namespace EventOrganizerAPI.Middleware
{
    public class ErrorHandlingMiddleware : IMiddleware
    {
        private readonly ILogger<ErrorHandlingMiddleware> _logger;

        public ErrorHandlingMiddleware(ILogger<ErrorHandlingMiddleware> logger)
        {
            _logger = logger;
        }
        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            try
            {
                await next.Invoke(context);
            }
            catch (NotFoundException e)
            {
                context.Response.StatusCode = 404;
                await context.Response.WriteAsync(e.Message);
            }
            catch (AuthenticationException ae)
            {
                context.Response.StatusCode = 401;
                await context.Response.WriteAsync(ae.Message);
            }
            catch (PermissionDeniedException pde)
            {
                context.Response.StatusCode = 401;
                await context.Response.WriteAsync(pde.Message);
            }
            catch (InvalidDateRangeException ide)
            {
                context.Response.StatusCode = 400;
                await context.Response.WriteAsync(ide.Message);
            }
            catch (InvalidOperationException ioe)
            {
                context.Response.StatusCode = 400;
                await context.Response.WriteAsync(ioe.Message);
            }
            catch (UserAlreadyParticipatingException uape)
            {
                context.Response.StatusCode = 400;
                await context.Response.WriteAsync(uape.Message);
            }
            catch (UserNotParticipatingException unape)
            {
                context.Response.StatusCode = 400;
                await context.Response.WriteAsync(unape.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);

                context.Response.StatusCode = 500;
                await context.Response.WriteAsync("Something wrong");
            }
        }
    }
}
